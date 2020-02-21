using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using dbexport.Common;
using dbexport.Interfaces;

namespace dbexport.FileGenerators
{
    public class HtmlGenerator : IGenerator
    {
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
            fileWriter.WriteLine(1, "<style>");
            fileWriter.WriteLine(2, "table {");
            fileWriter.WriteLine(3, "font-family: arial, sans-serif; border-collapse: collapse; width: 100%; }");
            fileWriter.WriteLine(2, "td, th {");
            fileWriter.WriteLine(3, "border: 1px solid #dddddd;text-align: left; padding: 8px; }");
            fileWriter.WriteLine(2, "tr:nth-child(even) {");
            fileWriter.WriteLine(3, "background-color: #dddddd;}");
            fileWriter.WriteLine(1, "</style>");
            fileWriter.WriteLine("</head>");
            fileWriter.WriteLine("<body>");
            fileWriter.WriteLine(1, "<table style='width:100%'>");
            fileWriter.WriteLine(1, "<h2>Table ", tableName, "</h2>");

            fileWriter.WriteLine(2, "<tr>");
            foreach (var column in columns)
            {
                fileWriter.WriteLine(3, "<th>", column.ColumnName, "</th>");
            }

            fileWriter.WriteLine(2, "</tr>");

            using var readData = reader.ReadData(connection, tableName, columnNames);
            while (readData.Read())
            {
                fileWriter.WriteLine(2, "<tr>");
                for (int i = 0; i < readData.FieldCount; i++)
                {
                    fileWriter.WriteLine(3, "<td>", readData.GetValue(i).ToString(), "</td>");
                }

                fileWriter.WriteLine(2,  "</tr>");
            }

            fileWriter.WriteLine(2, "</table>");
            fileWriter.WriteLine("</body>");
            fileWriter.WriteLine("</html>");
        }
    }
}