using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using dbexport.Interfaces;

namespace dbexport.FileGenerators
{
    public class XmlGenerator : IGenerator
    {
        public void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            //GenerateWithXmlWriter(reader, connection, tableName, path);
            GenerateWithXmlDocument(reader, connection, tableName, path);
        }

        private void GenerateWithXmlWriter(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            string filePath = Path.Combine(path, $"{tableName}.xml");
            var columns = reader.GetColumns(connection, tableName);
            var columnNames = columns.Select(x => x.ColumnName).ToArray();
            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using var writer = XmlWriter.Create(filePath, settings);
            
            writer.WriteStartElement(tableName);
            using var dataReader = reader.ReadData(connection, tableName, columnNames);
            while (dataReader.Read())
            {
                writer.WriteStartElement("Entity");
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    writer.WriteElementString(dataReader.GetName(i), dataReader.GetValue(i).ToString());
                }

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }

        private void GenerateWithXmlDocument(IDbExtractor reader, DbConnection connection, string tableName,
            string path)
        {
            string filePath = Path.Combine(path, $"{tableName}.xml");
            var columns = reader.GetColumns(connection, tableName);
            var columnNames = columns.Select(x => x.ColumnName).ToArray();
            
            XDocument document = new XDocument();
            XElement root = new XElement($"{tableName}");
            document.Add(root);

            using var dataReader = reader.ReadData(connection, tableName, columnNames);
            while (dataReader.Read())
            {
                XElement entity = new XElement("entity");
                root.Add(entity);
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    entity.Add(new XAttribute(dataReader.GetName(i), dataReader.GetValue(i)));
                }
            }

            document.Save(filePath);
        }
    }
}