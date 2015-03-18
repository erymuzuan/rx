using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_van.Domain
{
    public class Van : Entity
    {
        public Van()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "Van:" + Reg;
        }       //member:Reg
        [XmlAttribute]
        public string Reg { get; set; }

        //member:Model
        [XmlAttribute]
        public string Model { get; set; }

    }
}
