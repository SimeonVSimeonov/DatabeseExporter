﻿using System.Data.Common;
using dbexport.Interfaces;

namespace dbexport.DbCreators
{
    public class PostgresDbCreator : IDbCreator
    {
        public void Create(DbConnection connection)
        {
            if (connection == null)
                return;

            string[] commands =
            {
                "CREATE TABLE IF NOT EXISTS Countries (Id SERIAL PRIMARY KEY ,Name VARCHAR(50))",

                "CREATE TABLE IF NOT EXISTS Towns(Id SERIAL PRIMARY KEY ,Name VARCHAR(50) ,Country_Id INT )",

                "CREATE TABLE IF NOT EXISTS Students(Id SERIAL PRIMARY KEY ,Name VARCHAR(30) ,Age INT, Town_Id INT )",
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