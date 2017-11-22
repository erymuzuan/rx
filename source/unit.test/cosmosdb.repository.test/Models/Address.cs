using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.CosmosDb.Models
{
    public class Address : DomainObject
    {
        public Address()
        {
            var rc = new RuleContext(this);
        }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string State { get; set; }

        public string Postcode { get; set; }



    }
}