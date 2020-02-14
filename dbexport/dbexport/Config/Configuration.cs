namespace dbexport.Config
{
    public static class Configuration
    {
        public static string[] SuportedDb = new[] {"postgresql", "sqlite"};
        
        public const string NpgsqlConnectionString =
            "Host=127.0.0.1;Username=postgres;Password=test;Database=university";

        public const string SQLiteConnectionString = "Data Source=C:\\sqlite3\\db\\mydb.db;";

        public const string MariaDbConnectionString = "";
    }
}