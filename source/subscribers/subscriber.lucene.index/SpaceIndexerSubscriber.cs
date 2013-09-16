using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Messaging
{
    public class SpaceIndexerSubscriber : EntityIndexer<Space>
    {
        protected override Task<SearchMetadata> GetMetadata(Space item, MessageHeaders header)
        {
            var metadata = new SearchMetadata
                {
                    Title = item.RegistrationNo,
                    Text = item.ToString(),
                    Created = item.CreatedDate == DateTime.MinValue ? DateTime.Today : item.CreatedDate,
                    OwnerCode = item.CreatedBy,
                    Summary = item.ToString(),
                    Code = item.LotName,
                    Type = typeof(Space).Name,
                    Id = item.BuildingId
                };
            return Task.FromResult(metadata);
        }

    }
}