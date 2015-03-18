using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.DevV1_state.Domain
{
    public class State : Entity
    {
        public State()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "State:" + Name;
        }       //member:Name
        [XmlAttribute]
        public string Name { get; set; }

        //member:Abbreviation
        [XmlAttribute]
        public string Abbreviation { get; set; }

        //member:Country
        [XmlAttribute]
        public string Country { get; set; }

    }
}
