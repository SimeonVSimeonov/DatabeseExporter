using System.Data.Common;

namespace dbexport
{
    public interface IDbExtractor
    {
        void Extract(DbProviderFactory providerFactory);
    }
}