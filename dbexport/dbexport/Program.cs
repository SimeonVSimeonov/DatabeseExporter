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
                postgresDbCreator.Create(NpgsqlFactory.Instance);
                postgresDbSeeder.Seed(NpgsqlFactory.Instance);
                postgresDbExtractor.Extract(NpgsqlFactory.Instance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
        }
    }
}