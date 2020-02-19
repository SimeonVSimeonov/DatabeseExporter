using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using dbexport.Common;
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
                string tableName = (string) dataRow[2];
                tableNames[idx] = tableName;
                idx++;
            }

            return tableNames;
        }

        public DbColumnInfo[] GetColumns(DbConnection connection, string tableName)
        {
            List<DbColumnInfo> columns;

            using (NpgsqlCommand command = (NpgsqlCommand) connection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM \"{tableName}\" LIMIT 0";
                using NpgsqlDataReader reader = command.ExecuteReader(CommandBehavior.KeyInfo);
                columns = reader.GetColumnSchema().Select(column => new DbColumnInfo()
                {
                    ColumnName = column.ColumnName,
                    ColumnType = column.DataType,
                    IsNullable = column.AllowDBNull.GetValueOrDefault(),
                    IsPrimaryKey = column.IsKey.GetValueOrDefault()
                }).ToList();
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