using System.Data.Common;

namespace dbexport
{
    public interface IDbSeeder
    {
        void Seed(DbConnection connection);
    }
}