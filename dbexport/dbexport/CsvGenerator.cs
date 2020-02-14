using System;
using System.Data.Common;
using System.IO;
using dbexport.Interfaces;

namespace dbexport
{
    public class CsvGenerator : ICsvGenerator
    {
        public void Generate(IDbExtractor reader, DbConnection connection, string pgsqlTable, string path)
        {
            string filePath = Path.Combine(path, $"{pgsqlTable}.csv");
            using var fileStream = File.Create(filePath);
            using var fileWriter = new StreamWriter(fileStream);
            var columns = reader.GetColumns(connection, pgsqlTable);

            fileWriter.WriteLine(String.Join(",", columns));

            using var dataReader = reader.ReadData(connection, pgsqlTable, columns);
            while (dataReader.Read())
            {
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    fileWriter.Write(dataReader.GetValue(i));
                    if (i < dataReader.FieldCount - 1)
                    {
                        fileWriter.Write(",");
                    }
                }

                fileWriter.WriteLine();
            }
        }
    }
}