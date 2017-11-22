using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.CosmosDb.Models
{
    public class Spouse : DomainObject
    {
        public Spouse()
        {
            var rc = new RuleContext(this);
            this.WorkPlaceAddress = new Address();
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public Address WorkPlaceAddress { get; set; }


    }
}