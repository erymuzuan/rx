using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Applicant", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Applicant : DomainObject
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public string MyKad { get; set; }
        [XmlAttribute]
        public DateTime RegisteredDate { get; set; }
        public int? Age { get; set; }
        public DateTime? Dob { get; set; }
        public Vehicle Ride { get; set; }
        private readonly ObjectCollection<Car> m_Taxis = new ObjectCollection<Car>();
        public ObjectCollection<Car> Taxis { get { return m_Taxis; } }
        private readonly ObjectCollection<Vehicle> m_PastVehicles = new ObjectCollection<Vehicle>();
        public ObjectCollection<Vehicle> PastVehicles { get { return m_PastVehicles; } }
        private Address m_Address = new Address();
        public Address Address { get { return m_Address; } set { m_Address = value; } }
        private Contact m_Contact = new Contact();
        public Contact Contact { get { return m_Contact; } set { m_Contact = value; } }


    }
}
