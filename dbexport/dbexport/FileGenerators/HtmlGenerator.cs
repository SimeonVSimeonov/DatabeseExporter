using System;
using System.Data.Common;
using System.IO;
using System.Linq;
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
            
            fileWriter.WriteLine("<!DOCTYPE html>");
            fileWriter.WriteLine("<html>");
            fileWriter.WriteLine("<head>");
            fileWriter.WriteLine(Indentation(1) + "<style>");
            fileWriter.WriteLine(Indentation(2) + "table {");
            fileWriter.WriteLine(Indentation(3) + "font-family: arial, sans-serif; border-collapse: collapse; width: 100%; }");
            fileWriter.WriteLine(Indentation(2) + "td, th {");
            fileWriter.WriteLine(Indentation(3) + "border: 1px solid #dddddd;text-align: left; padding: 8px; }");
            fileWriter.WriteLine(Indentation(2) + "tr:nth-child(even) {");
            fileWriter.WriteLine(Indentation(3) + "background-color: #dddddd;}");
            fileWriter.WriteLine(Indentation(1) + "</style>");
            fileWriter.WriteLine("</head>");
            fileWriter.WriteLine("<body>");
            fileWriter.WriteLine(Indentation(1) + $"<h2>Table {tableName}</h2>");
            fileWriter.WriteLine(Indentation(1) + "<table style='width:100%'>");
            
            fileWriter.WriteLine(Indentation(2) + "<tr>");
            foreach (var column in columns)
            {
                fileWriter.Write(Indentation(3) + "<th>");
                fileWriter.Write($"{column}");
                fileWriter.WriteLine("</th>");
            }
            fileWriter.WriteLine(Indentation(2) + "</tr>");
            
            using var readData = reader.ReadData(connection, tableName, columns);
            while (readData.Read())
            {
                fileWriter.WriteLine(Indentation(2) + "<tr>");
                for (int i = 0; i < readData.FieldCount; i++)
                {
                    fileWriter.Write(Indentation(3) + "<td>");
                    fileWriter.Write($"{readData.GetValue(i)}");
                    fileWriter.WriteLine("</td>");
                }
                fileWriter.WriteLine(Indentation(2) + "</tr>");
            }
           
            fileWriter.WriteLine(Indentation(1) + "</table>");
            fileWriter.WriteLine("</body>");
            fileWriter.WriteLine("</html>");
        }

        private string Indentation(int count)
        {
            return String.Concat(Enumerable.Repeat("\t", count));
        }
    }
}