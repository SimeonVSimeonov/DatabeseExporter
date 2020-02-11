using System.Data.Common;

namespace dbexport
{
    public class SQLiteDbCreator : IDbCreator
    {
        public void Create(DbConnection connection)
        {
            if (connection == null)
                return;

            string[] commands =
            {
                "CREATE TABLE IF NOT EXISTS Countries (Id INTEGER PRIMARY KEY AUTOINCREMENT ,Name VARCHAR(50))",

                "CREATE TABLE IF NOT EXISTS Towns(Id INTEGER PRIMARY KEY AUTOINCREMENT ,Name VARCHAR(50) ,Country_Id INT )",

                "CREATE TABLE IF NOT EXISTS Students(Id INTEGER PRIMARY KEY AUTOINCREMENT ,Name VARCHAR(30) ,Age INT, Town_Id INT )",
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