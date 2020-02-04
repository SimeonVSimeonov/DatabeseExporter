using System;
using Npgsql;

namespace dbexport
{
    public static class Program
    {
        static void Main(string[] args)
        {
            PostgresDbCreator postgresDbCreator = new PostgresDbCreator();
            PostgresDbSeeder postgresDbSeeder = new PostgresDbSeeder();
            PostgresDbExtractor postgresDbExtractor = new PostgresDbExtractor();
            
            try
            {
                postgresDbCreator.Create(new NpgsqlConnection(Configuration.NpgsqlConnectionString));
                postgresDbSeeder.Seed(new NpgsqlConnection(Configuration.NpgsqlConnectionString));
                postgresDbExtractor.Extract(new NpgsqlConnection(Configuration.NpgsqlConnectionString));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}