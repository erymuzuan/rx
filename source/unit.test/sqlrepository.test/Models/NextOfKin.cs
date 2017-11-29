using System.Diagnostics;
using Bespoke.Sph.Domain;

namespace sqlrepository.test.Models
{
    [DebuggerDisplay("{FullName}")]
    public class NextOfKin : DomainObject
    {
        public string FullName { get; set; }
        public string Relationship { get; set; }
    }

    /*  "Wife": {
        "Name": "KAUR",
        "Age": 50,
        "WorkPlaceAddress": {
            "Street1": null,
            "Street2": null,
            "State": null,
            "Postcode": null,
            "WebId": null
        },
        "WebId": null
    }*/

    public class Spouse : DomainObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Address WorkPlaceAddress { get; set; }
    }
}