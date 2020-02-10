using System.Data;
using System.Data.Common;

namespace dbexport
{
    public class DbExtractor : IDbExtractor

    {
        public void Extract(DbConnection connection)
        {
            throw new System.NotImplementedException();
        }

        public string[] GetTables(DbConnection connection)
        {
            int idx = 0;
            string[] tableNames;

            using (connection)
            {
                connection.Open();
                DataTable dataTable = connection.GetSchema("Tables");
                tableNames = new string[dataTable.Rows.Count];
                foreach (DataRow dataRow in dataTable.Rows)
                {
                    string tableName = (string) dataRow[2];
                    tableNames[idx] = tableName;
                    idx++;
                }
            }

            return tableNames;
        }

        public string[] GetColumns(string tableName, DbConnection connection)
        {
            string[] columns;
            string query = $"select * from {tableName}";

            using (connection)
            {
                connection.Open();
                using (DbCommand command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        columns = new string[reader.FieldCount];
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            columns[i] = reader.GetName(i);
                        }
                    }
                }
            }
            
            return columns;
        }

        public DbDataReader ReadData(string tableName, string[] columns)
        {
            throw new System.NotImplementedException();
        }
    }
}