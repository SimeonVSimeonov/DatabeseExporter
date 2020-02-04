using System.Data;
using System.Data.Common;

namespace dbexport
{
    public class PostgresDbExtractor : IDbExtractor
    {
        public void Extract(DbProviderFactory providerFactory)
        {
            var connection = providerFactory.CreateConnection();
            if (connection == null)
                return;

            using (connection)
            {
                connection.ConnectionString = Configuration.NpgsqlConnectionString;
                connection.Open();
                
                DataTable dt = connection.GetSchema("Tables");
                foreach (DataRow row in dt.Rows)
                {
                    string schemaName = (string)row[1];
                    string tableName = (string) row[2];

                    string query = $"COPY {schemaName}.{tableName} TO 'D:\\{tableName}.csv' WITH (FORMAT CSV, HEADER);";
                    using (DbCommand command = connection.CreateCommand())
                    {
                        command.CommandText = query;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}