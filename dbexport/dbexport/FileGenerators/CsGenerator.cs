using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using dbexport.Common;
using dbexport.Interfaces;
using Pluralize.NET;

namespace dbexport.FileGenerators
{
    public class CsGenerator : IGenerator
    {
        IPluralize pluralizer = new Pluralizer();

        public void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            string pascalCaseName = ToPascalCase(tableName);
            string fileName = pluralizer.Singularize(pascalCaseName);
            string filePath = Path.Combine(path, $"{fileName}.cs");
            using var fileStream = File.Create(filePath);
            using var fileWriter = new StreamWriter(fileStream);

            var columns = reader.GetColumns(connection, tableName);

            fileWriter.WriteLine("using System.ComponentModel.DataAnnotations;");
            fileWriter.WriteLine("using System.ComponentModel.DataAnnotations.Schema;");
            fileWriter.WriteLine();
            fileWriter.WriteLine("namespace Entities");
            fileWriter.WriteLine("{");
            fileWriter.WriteLine(1, $"[Table(\"{tableName}\")]");
            fileWriter.WriteLine(1, $"public class {fileName}");
            fileWriter.WriteLine(1, "{");
            foreach (var columnInfo in columns)
            {
                var propName = ToPascalCase(columnInfo.ColumnName);
                var type = TypeNameOrAlias(columnInfo.ColumnType);
                if (columnInfo.IsPrimaryKey)
                {
                    fileWriter.WriteLine(2, "[Key]");
                }

                fileWriter.WriteLine(2, $"[Column(\"{propName}\")]");
                if (columnInfo.IsNullable && columnInfo.ColumnType != typeof(string))
                {
                    type += "?";
                }

                fileWriter.WriteLine(2, "public ", type, " ", propName,
                    " { get; set; }");
                
                var last = columns.Last();
                if (!columnInfo.Equals(last))
                {
                    fileWriter.WriteLine();
                }
            }

            fileWriter.WriteLine(1, "}");
            fileWriter.WriteLine("}");
        }

        static Dictionary<Type, string> typeAlias = new Dictionary<Type, string>
        {
            {typeof(bool), "bool"},
            {typeof(byte), "byte"},
            {typeof(char), "char"},
            {typeof(decimal), "decimal"},
            {typeof(double), "double"},
            {typeof(float), "float"},
            {typeof(int), "int"},
            {typeof(long), "long"},
            {typeof(sbyte), "sbyte"},
            {typeof(short), "short"},
            {typeof(string), "string"},
            {typeof(uint), "uint"},
            {typeof(ulong), "ulong"},
        };

        static string TypeNameOrAlias(Type type)
        {
            if (typeAlias.TryGetValue(type, out string alias))
                return alias;

            return type.Name;
        }

        private string ToPascalCase(string text)
        {
            var words = text
                .Split(new[] {'-', '_'}, StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word.Substring(0, 1).ToUpper()
                                + word.Substring(1));

            return String.Concat(words);
        }
    }
}