using Npgsql;

namespace dbexport
{
    class Program
    {
        static void Main(string[] args)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(Configuration.NpgsqlConnectionString))
            {
                connection.Open();

                string[] createStatements =
                {
                    "CREATE TABLE Countries (Id SERIAL PRIMARY KEY ,Name VARCHAR(50))",
                    
                    "CREATE TABLE Towns(Id SERIAL PRIMARY KEY ,Name VARCHAR(50) ,CountryCode INT REFERENCES Countries(Id))",
                    
                    "CREATE TABLE Students(Id SERIAL PRIMARY KEY ,Name VARCHAR(30) ,Age INT, TownId INT REFERENCES Towns(Id))",
                };
                
                foreach (var statement in createStatements)
                {
                    ExecNonQuery(connection, statement);
                }

                string[] insertStatements =
                {
                    "INSERT INTO Countries (NAME) VALUES ('Bulgaria'),('England'),('USA'),('Cyprus'),('Germany'),('Norway'),('Italy')",
                    
                    "INSERT INTO Towns (NAME, CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('San Francisco', 3),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 5),('Frankfurt', 5),('Oslo', 6), ('Naples', 7)",
                    
                    "INSERT INTO Students (Name, Age, TownId) VALUES ('Pesho Peshev', 21, 3)",
                    
                    "INSERT INTO Students (Name, Age, TownId) VALUES ('Gosho Geshev', 35, 1),('John Balon', 45, 5),('Cow Boy', 23, 6),('Francesco Neapolitano' , 31, 13)",
                };

                foreach (var statement in insertStatements)
                {
                    ExecNonQuery(connection, statement);
                }
            }
        }

        private static void ExecNonQuery(NpgsqlConnection connection, string cmdText)
        {
            using (NpgsqlCommand command = new NpgsqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}