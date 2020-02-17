using System;
using System.Data.Common;
using System.IO;
using dbexport.Interfaces;

namespace dbexport
{
    public class CsvGenerator : IGenerator
    {
        public void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            string filePath = Path.Combine(path, $"{tableName}.csv");
            using var fileStream = File.Create(filePath);
            using var fileWriter = new StreamWriter(fileStream);
            var columns = reader.GetColumns(connection, tableName);

            fileWriter.WriteLine(String.Join(",", columns));

            using var dataReader = reader.ReadData(connection, tableName, columns);
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