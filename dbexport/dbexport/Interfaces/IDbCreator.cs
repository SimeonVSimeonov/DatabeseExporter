using System.Data.Common;

namespace dbexport.Interfaces
{
    public interface IDbCreator
    {
        void Create(DbConnection connection);
    }
}