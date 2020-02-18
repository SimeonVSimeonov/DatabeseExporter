using System;

namespace dbexport.Common
{
    public class DbColumnInfo
    {
        public string ColumnName { get; set; }
        public bool IsNullable { get; set; }
        public Type ColumnType { get; set; }    
    }
}