using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Dev_appointment.Domain
{
    public class Appointment : Entity
    {
        public Appointment()
        {
            var rc = new RuleContext(this);
        }

        public override string ToString()
        {
            return "Appointment:" + ReferenceNo;
        }       //member:ReferenceNo
        [XmlAttribute]
        public string ReferenceNo { get; set; }

        //member:Mrn
        [XmlAttribute]
        public string Mrn { get; set; }

        //member:DateTime
        [XmlAttribute]
        public DateTime DateTime { get; set; }

        //member:Doctor
        [XmlAttribute]
        public string Doctor { get; set; }

        //member:Ward
        [XmlAttribute]
        public string Ward { get; set; }

        //member:Location
        [XmlAttribute]
        public string Location { get; set; }

        //member:Note
        [XmlAttribute]
        public string Note { get; set; }

        //member:Referral
        [XmlAttribute]
        public string Referral { get; set; }

    }
}
