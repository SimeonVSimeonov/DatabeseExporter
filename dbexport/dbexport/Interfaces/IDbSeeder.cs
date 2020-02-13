using System.Data.Common;

namespace dbexport.Interfaces
{
    public interface IDbSeeder
    {
        void Seed(DbConnection connection);
    }
}