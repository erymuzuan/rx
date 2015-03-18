using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_vehicle.Domain
{
    public class Vehicle : Entity
    {
        public Vehicle()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "Vehicle:" + No;
        }       //member:No
        [XmlAttribute]
        public string No { get; set; }

        //member:Make
        [XmlAttribute]
        public string Make { get; set; }

        //member:Model
        [XmlAttribute]
        public string Model { get; set; }

        //member:Category
        [XmlAttribute]
        public string Category { get; set; }

        //member:ModelYear
        [XmlAttribute]
        public int ModelYear { get; set; }

        //member:RetailPrice
        public decimal? RetailPrice { get; set; }

        //member:Seating
        [XmlAttribute]
        public int Seating { get; set; }

        //member:Weight
        [XmlAttribute]
        public decimal Weight { get; set; }

        //member:OriginCountry
        [XmlAttribute]
        public string OriginCountry { get; set; }

    }
}
