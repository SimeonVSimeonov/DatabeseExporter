using System;
using System.Data.SQLite; //Not Microsoft.Data.Sqlite; new SqliteConnection();
using System.Linq;
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

            DbExtractor dbExtractor = new DbExtractor();
            try
            {
                //TODO: pass data from GetTables method to GetColumns
                var pgsqlCol = dbExtractor.GetColumns("university.students",
                    new NpgsqlConnection(Configuration.NpgsqlConnectionString));
                Console.WriteLine(string.Join(", ", pgsqlCol));
   
                var sqliteCol = dbExtractor.GetColumns("Students",
                    new SQLiteConnection(Configuration.SQLiteConnectionString));
                Console.WriteLine(string.Join(", ", sqliteCol));

                var pgsql = dbExtractor.GetTables(new NpgsqlConnection(Configuration.NpgsqlConnectionString));
                Console.WriteLine(string.Join(", ", pgsql));
                
                var sqlite = dbExtractor.GetTables(new SQLiteConnection(Configuration.SQLiteConnectionString));
                Console.WriteLine(string.Join(", ", sqlite));

                // postgresDbCreator.Create(new NpgsqlConnection(Configuration.NpgsqlConnectionString));
                // postgresDbSeeder.Seed(new NpgsqlConnection(Configuration.NpgsqlConnectionString));

                // sqLiteDbCreator.Create(new SQLiteConnection(Configuration.SQLiteConnectionString));
                // sqLiteDbSeeder.Seed(new SQLiteConnection(Configuration.SQLiteConnectionString));    
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}