using System.Data.Common;
using Npgsql;

namespace dbexport
{
    public class PostgresDbCreator : IDbCreator
    {
        public void Create(DbProviderFactory providerFactory)
        {
            var connection = providerFactory.CreateConnection();
            if (connection == null)
                return;

            string[] commands =
            {
                "CREATE SCHEMA IF NOT EXISTS university",
                
                "CREATE TABLE IF NOT EXISTS university.Countries (Id SERIAL PRIMARY KEY ,Name VARCHAR(50))",

                "CREATE TABLE IF NOT EXISTS university.Towns(Id SERIAL PRIMARY KEY ,Name VARCHAR(50) ,Country_Id INT )",

                "CREATE TABLE IF NOT EXISTS university.Students(Id SERIAL PRIMARY KEY ,Name VARCHAR(30) ,Age INT, Town_Id INT )",
            };
            
            using (connection)
            {
                connection.ConnectionString = Configuration.NpgsqlConnectionString;
                connection.Open();

                foreach (var statement in commands)
                {
                    ExecNonQuery(connection, statement);
                }
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