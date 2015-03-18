using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_district.Domain
{
    public class District : Entity
    {
        public District()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "District:" + Name;
        }       //member:Name
        [XmlAttribute]
        public string Name { get; set; }

        //member:Postcode
        [XmlAttribute]
        public string Postcode { get; set; }

        //member:State
        [XmlAttribute]
        public string State { get; set; }

    }
}
