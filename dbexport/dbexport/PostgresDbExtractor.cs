using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using Npgsql;

namespace dbexport
{
    public class PostgresDbExtractor : IDbExtractor
    {
        public void Extract(DbConnection connection)
        {
            if (connection == null)
                return;

            StringBuilder stringBuilder = new StringBuilder();

            using (connection)
            {
                connection.Open();

                DataTable dt = connection.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    string schemaName = (string) row[1];
                    string tableName = (string) row[2];

                    string query = $"select * from {schemaName}.{tableName}";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, (NpgsqlConnection) connection))
                    {
                        using (NpgsqlDataReader dataReader = command.ExecuteReader())
                        {
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                stringBuilder.Append(dataReader.GetName(i) + ",");
                            }
                            
                            stringBuilder.AppendLine();
                            
                            while (dataReader.Read())
                            {
                                for (int i = 0; i < dataReader.FieldCount; i++)
                                {
                                    stringBuilder.Append(dataReader[i] + ",");
                                }
                                
                                stringBuilder.AppendLine();
                            }
                        }
                    }
                }
            }

            File.WriteAllText("D:\\data.csv", stringBuilder.ToString());
        }
    }
}