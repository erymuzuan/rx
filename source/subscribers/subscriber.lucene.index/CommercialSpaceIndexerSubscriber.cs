using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class CommercialSpaceIndexerSubscriber : EntityIndexer<CommercialSpace>
    {
        protected override Task<SearchMetadata> GetMetadata(CommercialSpace item, MessageHeaders header)
        {
            var metadata = new SearchMetadata
                {
                    Title = item.RegistrationNo,
                    Text = item.ToString(),
                    Created = item.CreatedDate == DateTime.MinValue ? DateTime.Today : item.CreatedDate,
                    OwnerCode = item.CreatedBy,
                    Summary = item.ToString(),
                    Code = item.LotName,
                    Type = typeof(CommercialSpace).Name,
                    Id = item.BuildingId
                };
            return Task.FromResult(metadata);
        }

    }
}