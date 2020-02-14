using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using dbexport.Interfaces;
using Npgsql;

namespace dbexport.DbExtractors
{
    public class PostgresDbExtractor : IDbExtractor
    {
        public string[] GetTables(DbConnection connection)
        {
            int idx = 0;
            string[] tableNames;

            DataTable dataTable = connection.GetSchema("Tables");
            tableNames = new string[dataTable.Rows.Count];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                //string schemaName = (string) dataRow[1];
                string tableName = (string) dataRow[2];
                tableNames[idx] = tableName;
                idx++;
            }

            return tableNames;
        }

        public string[] GetColumns(DbConnection connection, string tableName)
        {
            List<string> columns = new List<string>();

            using (NpgsqlCommand command = (NpgsqlCommand)connection.CreateCommand())
            {
                command.CommandText =
                    $"SELECT column_name FROM information_schema.columns WHERE table_name = '{tableName}'";
                // NpgsqlParameter parameter = new NpgsqlParameter("tableName", DbType.String);
                // parameter.Value = tableName;
                // command.Parameters.Add(parameter);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(reader.GetString(0));
                    }
                }
            }

            return columns.ToArray();
        }

        public DbDataReader ReadData(DbConnection connection, string tableName, string[] columns)
        {
            DbDataReader reader = null;
            DbCommand command = null;

            command = connection.CreateCommand();
            command.CommandText = $"SELECT {String.Join(", ", columns)} FROM {tableName}";
            reader = command.ExecuteReader();
            return reader;
        }
    }
}