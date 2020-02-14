using System.Data.Common;

namespace dbexport.Interfaces
{
    public interface ICsvGenerator
    {
        void Generate(IDbExtractor reader, DbConnection connection, string tableName, string path);
    }
}