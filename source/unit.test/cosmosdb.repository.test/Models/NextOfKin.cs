using Bespoke.Sph.Domain;
using Address = Bespoke.Sph.Tests.CosmosDb.Models.Address;

namespace Bespoke.Sph.Tests.CosmosDb.Models
{
    public class NextOfKin : DomainObject
    {
        public NextOfKin()
        {
            var rc = new RuleContext(this);
        }

        public string FullName { get; set; }

        public string Relationship { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }



    }
}