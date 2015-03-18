using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_telephone.Domain
{
    public class Telephone : Entity
    {
        public Telephone()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "Telephone:" + Model;
        }       //member:Model
        [XmlAttribute]
        public string Model { get; set; }

        //member:Name
        [XmlAttribute]
        public string Name { get; set; }

    }
}
