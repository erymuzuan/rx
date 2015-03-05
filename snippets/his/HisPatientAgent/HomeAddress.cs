
using System.Xml.Serialization;

namespace Bespoke.DevV1_patient.Domain
{
    public class HomeAddress 
    {
        public HomeAddress()
        {
        }
        [XmlAttribute]
        public string Street { get; set; }

        [XmlAttribute]
        public string Street2 { get; set; }

        [XmlAttribute]
        public string Postcode { get; set; }

        [XmlAttribute]
        public string City { get; set; }

        [XmlAttribute]
        public string State { get; set; }

        [XmlAttribute]
        public string Country { get; set; }

    }







}
