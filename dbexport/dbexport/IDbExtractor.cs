﻿using System.Data.Common;

namespace dbexport
{
    public interface IDbExtractor
    {
        string[] GetTables(DbConnection connection);
        string[] GetColumns(DbConnection connection, string tableName);
        DbDataReader ReadData(DbConnection connection, string tableName, string[] columns);
    }
}