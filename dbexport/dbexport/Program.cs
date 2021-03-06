﻿using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using dbexport.Config;
using dbexport.DbCreators;
using dbexport.DbExtractors;
using dbexport.DbSeeders;
using dbexport.FileGenerators;
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
            HtmlGenerator htmlGenerator = new HtmlGenerator();
            XmlGenerator xmlGenerator = new XmlGenerator();
            CsGenerator csGenerator = new CsGenerator();

            try
            {
                using (DbConnection connection = new SQLiteConnection(Configuration.SQLiteConnectionString))
                {
                    connection.Open();

                    var path = "D:\\sqLite_output";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    Directory.CreateDirectory(path);

                    var sqLiteTables = sqLiteDbExtractor.GetTables(connection);
                    foreach (var sqLiteTable in sqLiteTables.Where(x => x != "sqlite_sequence"))
                    {
                        csvGenerator.Generate(sqLiteDbExtractor, connection, sqLiteTable, path);
                        htmlGenerator.Generate(sqLiteDbExtractor, connection, sqLiteTable, path);
                        xmlGenerator.Generate(sqLiteDbExtractor, connection, sqLiteTable, path);
                        csGenerator.Generate(sqLiteDbExtractor, connection, sqLiteTable, path);
                    }
                }

                using (DbConnection connection = new NpgsqlConnection(Configuration.NpgsqlConnectionString))
                {
                    connection.Open();

                    var path = "D:\\pgsql_output";
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    Directory.CreateDirectory(path);

                    var pgsqlTables = postgresDbExtractor.GetTables(connection);
                    foreach (var pgsqlTable in pgsqlTables)
                    {
                        csvGenerator.Generate(postgresDbExtractor, connection, pgsqlTable, path);
                        htmlGenerator.Generate(postgresDbExtractor, connection, pgsqlTable, path);
                        xmlGenerator.Generate(postgresDbExtractor, connection, pgsqlTable, path);
                        csGenerator.Generate(postgresDbExtractor, connection, pgsqlTable, path);
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