using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using dbexport.Interfaces;

namespace dbexport.FileGenerators
{
    public class HtmlGenerator : IGenerator
    {
        Dictionary<int, string> indentCache = new Dictionary<int, string>();
        
        public void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            string filePath = Path.Combine(path, $"{tableName}.html");
            using var fileStream = File.Create(filePath);
            using var fileWriter = new StreamWriter(fileStream);

            var columns = reader.GetColumns(connection, tableName);
            var columnNames = columns.Select(x => x.ColumnName).ToArray();

            fileWriter.WriteLine("<!DOCTYPE html>");
            fileWriter.WriteLine("<html>");
            fileWriter.WriteLine("<head>");
            Write(fileWriter, 1, "<style>");
            Write(fileWriter, 2, "table {");
            Write(fileWriter, 3, "font-family: arial, sans-serif; border-collapse: collapse; width: 100%; }");
            Write(fileWriter, 2, "td, th {");
            Write(fileWriter, 3, "border: 1px solid #dddddd;text-align: left; padding: 8px; }");
            Write(fileWriter, 2, "tr:nth-child(even) {");
            Write(fileWriter, 3, "background-color: #dddddd;}");
            Write(fileWriter, 1, "</style>");
            fileWriter.WriteLine("</head>");
            fileWriter.WriteLine("<body>");
            Write(fileWriter,1, "<table style='width:100%'>");
            Write(fileWriter, 1, "<h2>Table ", tableName, "</h2>");

            Write(fileWriter, 2, "<tr>");
            foreach (var column in columns)
            {
                Write(fileWriter, 3, "<th>", column.ColumnName, "</th>");
            }

            Write(fileWriter, 2, "</tr>");

            using var readData = reader.ReadData(connection, tableName, columnNames);
            while (readData.Read())
            {
                Write(fileWriter, 2, "<tr>");
                for (int i = 0; i < readData.FieldCount; i++)
                {
                    Write(fileWriter, 3, "<td>", readData.GetValue(i).ToString(), "</td>");
                }

                Write(fileWriter, 2,  "</tr>");
            }

            Write(fileWriter, 2, "</table>");
            fileWriter.WriteLine("</body>");
            fileWriter.WriteLine("</html>");
        }

        private static string Indentation(int count)
        {
            return new String('\t', count);
        }
        
        private void Write(StreamWriter writer, int indent, params string[] values)
        {
            if (!indentCache.TryGetValue(indent, out string indentation))
            {
                indentation = Indentation(indent);
                indentCache.Add(indent, indentation);
            }
            
            writer.Write(indentation);
            foreach (var value in values)
            {
                writer.Write(value);
            }

            writer.WriteLine();
        }
    }
}