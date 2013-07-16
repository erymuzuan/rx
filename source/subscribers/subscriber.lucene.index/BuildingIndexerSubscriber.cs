using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class BuildingIndexerSubscriber : EntityIndexer<Building>
    {
        protected override Task<SearchMetadata> GetMetadata(Building item, MessageHeaders header)
        {
            var metadata = new SearchMetadata
                  {
                      Title = item.Name,
                      Text = item.ToString(),
                      Created = item.CreatedDate,
                      OwnerCode = item.CreatedBy,
                      Summary = item.ToString(),
                      Code = item.LotNo,
                      Type = typeof(Building).Name,
                      Id = item.BuildingId
                  };
            return Task.FromResult(metadata);
        }

    }
}
