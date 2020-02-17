using System.Data.Common;
using System.IO;
using System.Xml;
using dbexport.Interfaces;

namespace dbexport.FileGenerators
{
    public class XmlGenerator : IGenerator
    {
        public void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path)
        {
            string filePath = Path.Combine(path, $"{tableName}.xml");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            using var writer = XmlWriter.Create(filePath, settings);
            var columns = reader.GetColumns(connection, tableName);

            writer.WriteStartElement(tableName);
            using var dataReader = reader.ReadData(connection, tableName, columns);
            while (dataReader.Read())
            {
                writer.WriteStartElement("Entity"); //TODO can be more specific
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    writer.WriteElementString(dataReader.GetName(i), dataReader.GetValue(i).ToString());
                }

                writer.WriteEndElement();
            }
            
            writer.WriteEndElement();
        }
    }
}