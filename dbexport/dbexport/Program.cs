using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using dbexport.Config;
using dbexport.DbCreators;
using dbexport.DbExtractors;
using dbexport.DbSeeders;
using Npgsql;

namespace dbexport
{
    public static class Program
    {
        static void Main(string[] args)
        {
            PostgresDbCreator postgresDbCreator = new PostgresDbCreator();
            PostgresDbSeeder postgresDbSeeder = new PostgresDbSeeder();

            SQLiteDbCreator sqLiteDbCreator = new SQLiteDbCreator();
            SQLiteDbSeeder sqLiteDbSeeder = new SQLiteDbSeeder();

            PostgresDbExtractor postgresDbExtractor = new PostgresDbExtractor();
            SQLiteDbExtractor sqLiteDbExtractor = new SQLiteDbExtractor();

            Dictionary<string, string[]> tableData = new Dictionary<string, string[]>();

            try
            {
                using (DbConnection connection = new SQLiteConnection(Configuration.SQLiteConnectionString))
                {
                    connection.Open();

                    //sqLiteDbCreator.Create(connection);
                    //sqLiteDbSeeder.Seed(connection);

                    var sqliteTables = sqLiteDbExtractor.GetTables(connection);
                    foreach (var sqliteTable in sqliteTables)
                    {
                        var columns = sqLiteDbExtractor.GetColumns(connection, sqliteTable);
                        using var data = sqLiteDbExtractor.ReadData(connection, sqliteTable, columns);
                        tableData.Add(sqliteTable, columns);
                    }
                }
                
                using (DbConnection connection = new NpgsqlConnection(Configuration.NpgsqlConnectionString))
                {
                    connection.Open();

                    //postgresDbCreator.Create(connection);
                    //postgresDbSeeder.Seed(connection);

                    var pgsqlTables = postgresDbExtractor.GetTables(connection);
                    foreach (var pgsqlTable in pgsqlTables)
                    {
                        var columns = postgresDbExtractor.GetColumns(connection, pgsqlTable);
                        using var data = postgresDbExtractor.ReadData(connection, pgsqlTable, columns);
                        tableData.Add(pgsqlTable, columns);
                    }
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}