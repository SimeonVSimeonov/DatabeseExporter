using System.Data.Common;

namespace dbexport
{
    public interface IDbCreator
    {
        void Create(DbConnection connection);
    }
}