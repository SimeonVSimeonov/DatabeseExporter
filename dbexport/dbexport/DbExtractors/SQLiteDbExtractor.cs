using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using dbexport.Common;
using dbexport.Interfaces;

namespace dbexport.DbExtractors
{
    public class SQLiteDbExtractor : IDbExtractor
    {
        public string[] GetTables(DbConnection connection)
        {
            int idx = 0;
            string[] tableNames;

            DataTable dataTable = connection.GetSchema("Tables");
            tableNames = new string[dataTable.Rows.Count];
            foreach (DataRow dataRow in dataTable.Rows)
            {
                string tableName = (string) dataRow[2];
                tableNames[idx] = tableName;
                idx++;
            }

            return tableNames;
        }

        public DbColumnInfo[] GetColumns(DbConnection connection, string tableName)
        {
            List<DbColumnInfo> columns = new List<DbColumnInfo>();

            using (DbCommand command = connection.CreateCommand())
            {
                command.CommandText = $"PRAGMA table_info ('{tableName}')";
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(new DbColumnInfo()
                        {
                            ColumnName = reader.GetString(1),
                        });
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