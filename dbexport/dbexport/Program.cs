using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
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
            CsvGenerator csvGenerator = new CsvGenerator();

            try
            {
                using (DbConnection connection = new SQLiteConnection(Configuration.SQLiteConnectionString))
                {
                    connection.Open();
                    
                    var path = "D:\\output";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                    
                    var tables = sqLiteDbExtractor.GetTables(connection);
                    foreach (var table in tables.Where(x => x != "sqlite_sequence"))
                    {
                        csvGenerator.Generate(sqLiteDbExtractor, connection, table, path);
                    }
                
                }
                
                using (DbConnection connection = new NpgsqlConnection(Configuration.NpgsqlConnectionString))
                {
                    connection.Open();

                    var path = "D:\\output";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    Directory.CreateDirectory(path);

                    var pgsqlTables = postgresDbExtractor.GetTables(connection);
                    foreach (var pgsqlTable in pgsqlTables)
                    {
                        csvGenerator.Generate(postgresDbExtractor, connection, pgsqlTable, path);
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