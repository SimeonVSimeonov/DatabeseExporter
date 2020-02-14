using System.Data.Common;
using dbexport.Interfaces;

namespace dbexport.DbSeeders
{
    public class PostgresDbSeeder : IDbSeeder

    {
        public void Seed(DbConnection connection)
        {
            if (connection == null)
                return;
            
            string[] commands =
            {
                "TRUNCATE TABLE Countries RESTART IDENTITY",

                "TRUNCATE TABLE Towns RESTART IDENTITY",

                "TRUNCATE TABLE Students RESTART IDENTITY",

                "INSERT INTO Countries (NAME) VALUES ('Bulgaria'),('England'),('USA'),('Cyprus'),('Germany'),('Norway'),('Italy')",

                "INSERT INTO Towns (NAME, Country_Id) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('San Francisco', 3),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 5),('Frankfurt', 5),('Oslo', 6), ('Naples', 7)",

                "INSERT INTO Students (Name, Age, Town_Id) VALUES ('Pesho Peshev', 21, 3)",

                "INSERT INTO Students (Name, Age, Town_Id) VALUES ('Gosho Geshev', 35, 1),('John Balon', 45, 5),('Cow Boy', 23, 6),('Francesco Neapolitano' , 31, 13)",
            };

            foreach (var statement in commands)
            {
                ExecNonQuery(connection, statement);
            }
        }

        private void ExecNonQuery(DbConnection connection, string statement)
        {
            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = statement;
                command.ExecuteNonQuery();
            }
        }
    }
}