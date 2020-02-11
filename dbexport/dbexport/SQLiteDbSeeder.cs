using System.Data.Common;

namespace dbexport
{
    public class SQLiteDbSeeder : IDbSeeder

    {
        public void Seed(DbConnection connection)
        {
            if (connection == null)
                return;
            
            string[] commands =
            {
                "DELETE FROM Countries",

                "DELETE FROM SQLITE_SEQUENCE WHERE name='Countries'",

                "DELETE FROM Towns",

                "DELETE FROM SQLITE_SEQUENCE WHERE name='Towns'",

                "DELETE FROM Students",

                "DELETE FROM SQLITE_SEQUENCE WHERE name='Students'",

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