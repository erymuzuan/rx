using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.CosmosDb.Models
{
    public class CustomHomeAddress : DomainObject
    {
        public CustomHomeAddress()
        {
            var rc = new RuleContext(this);
        }

        public string Street { get; set; }

        public string Street2 { get; set; }

        public string Postcode { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }



    }

}