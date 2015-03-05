using System;
using System.Xml.Serialization;

namespace Bespoke.DevV1_patient.Domain
{
    public class Patient 
    {
        public Patient()
        {
            this.NextOfKin = new NextOfKin();
            this.HomeAddress = new HomeAddress();
        }

        public string Id { get; set; }
        public override string ToString()
        {
            return "Patient:" + Mrn;
        }       //member:Mrn
        [XmlAttribute]
        public string Mrn { get; set; }

        //member:FullName
        [XmlAttribute]
        public string FullName { get; set; }

        //member:Dob
        [XmlAttribute]
        public DateTime Dob { get; set; }

        //member:Gender
        [XmlAttribute]
        public string Gender { get; set; }

        //member:Religion
        [XmlAttribute]
        public string Religion { get; set; }

        //member:Race
        [XmlAttribute]
        public string Race { get; set; }

        //member:RegisteredDate
        [XmlAttribute]
        public DateTime RegisteredDate { get; set; }

        //member:IdentificationNo
        [XmlAttribute]
        public string IdentificationNo { get; set; }

        //member:PassportNo
        [XmlAttribute]
        public string PassportNo { get; set; }

        //member:Nationality
        [XmlAttribute]
        public string Nationality { get; set; }

        //member:NextOfKin
        public NextOfKin NextOfKin { get; set; }

        //member:HomeAddress
        public HomeAddress HomeAddress { get; set; }

        //member:Occupation
        [XmlAttribute]
        public string Occupation { get; set; }

        //member:Status
        [XmlAttribute]
        public string Status { get; set; }

        //member:Age
        [XmlAttribute]
        public int Age { get; set; }

        //member:Income
        [XmlAttribute]
        public decimal Income { get; set; }

        //member:Empty
        [XmlAttribute]
        public string Empty { get; set; }

        //member:Spouse
        [XmlAttribute]
        public string Spouse { get; set; }

        //member:MaritalStatus
        [XmlAttribute]
        public string MaritalStatus { get; set; }

        //member:OldIC
        [XmlAttribute]
        public string OldIC { get; set; }

    }
}
