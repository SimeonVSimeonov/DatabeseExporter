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
        Formatter formatter = new Formatter();
        
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
            formatter.Write(fileWriter, 1, "<style>");
            formatter.Write(fileWriter, 2, "table {");
            formatter.Write(fileWriter, 3, "font-family: arial, sans-serif; border-collapse: collapse; width: 100%; }");
            formatter.Write(fileWriter, 2, "td, th {");
            formatter.Write(fileWriter, 3, "border: 1px solid #dddddd;text-align: left; padding: 8px; }");
            formatter.Write(fileWriter, 2, "tr:nth-child(even) {");
            formatter.Write(fileWriter, 3, "background-color: #dddddd;}");
            formatter.Write(fileWriter, 1, "</style>");
            fileWriter.WriteLine("</head>");
            fileWriter.WriteLine("<body>");
            formatter.Write(fileWriter,1, "<table style='width:100%'>");
            formatter.Write(fileWriter, 1, "<h2>Table ", tableName, "</h2>");

            formatter.Write(fileWriter, 2, "<tr>");
            foreach (var column in columns)
            {
                formatter.Write(fileWriter, 3, "<th>", column.ColumnName, "</th>");
            }

            formatter.Write(fileWriter, 2, "</tr>");

            using var readData = reader.ReadData(connection, tableName, columnNames);
            while (readData.Read())
            {
                formatter.Write(fileWriter, 2, "<tr>");
                for (int i = 0; i < readData.FieldCount; i++)
                {
                    formatter.Write(fileWriter, 3, "<td>", readData.GetValue(i).ToString(), "</td>");
                }

                formatter.Write(fileWriter, 2,  "</tr>");
            }

            formatter.Write(fileWriter, 2, "</table>");
            fileWriter.WriteLine("</body>");
            fileWriter.WriteLine("</html>");
        }
    }
}