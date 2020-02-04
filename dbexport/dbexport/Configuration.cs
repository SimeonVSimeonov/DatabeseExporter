namespace dbexport
{
    public static class Configuration
    {
        public static string[] SuportedDb = new[] {"postgresql"};
        
        public const string NpgsqlConnectionString =
            "Host=127.0.0.1;Username=postgres;Password=test;Database=postgres";

        public const string MariaDbConnectionString = "";
    }
}