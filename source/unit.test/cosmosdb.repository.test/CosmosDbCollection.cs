using Xunit;

namespace Bespoke.Sph.Tests.CosmosDb
{
    [CollectionDefinition(COSMOSDB_COLLECTION)]
    public class CosmosDbCollection : ICollectionFixture<CosmosDbFixture>
    {
        public const string COSMOSDB_COLLECTION = "CosmosDb collection";
    }
}