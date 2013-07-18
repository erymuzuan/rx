using System;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Messaging
{
    public class RentalApplicationIndexerSubscriber : EntityIndexer<RentalApplication>
    {
        protected override Task<SearchMetadata> GetMetadata(RentalApplication item, MessageHeaders header)
        {
            var metadata = new SearchMetadata
                {
                    Title = item.RegistrationNo + " : " + item.CompanyName ,
                    Text = item.ToString(),
                    Created = item.CreatedDate == DateTime.MinValue ? DateTime.Today : item.CreatedDate,
                    OwnerCode = item.CreatedBy,
                    Summary = item.ToString(),
                    Code = item.RegistrationNo,
                    Type = typeof(CommercialSpace).Name,
                    Id = item.RentalApplicationId
                };
            return Task.FromResult(metadata);
        }

    }
}