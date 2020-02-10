
using System.Data.Common;

namespace dbexport
{
    public interface IDbExtractor
    {
        void Extract(DbConnection connection);
        string[] GetTables(DbConnection connection);
        string[] GetColumns(string tableName, DbConnection connection);
        DbDataReader ReadData(string tableName, string[] columns);
    }
}