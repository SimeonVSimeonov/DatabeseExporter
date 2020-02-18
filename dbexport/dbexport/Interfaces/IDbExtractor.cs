using System.Data.Common;
using dbexport.Common;

namespace dbexport.Interfaces
{
    public interface IDbExtractor
    {
        string[] GetTables(DbConnection connection);
        DbColumnInfo[] GetColumns(DbConnection connection, string tableName);
        DbDataReader ReadData(DbConnection connection, string tableName, string[] columns);
    }
}