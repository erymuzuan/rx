using Bespoke.Sph.Domain;
using System;
using System.Xml.Serialization;

namespace Bespoke.Sph.Workflows_TryAndCatch_0
{
    [XmlType("Contact", Namespace = "http://www.maim.gov.my/wakaf")]
    public class Contact : DomainObject
    {
        [XmlAttribute]
        public string Telephone { get; set; }
        private Address m_Address = new Address();
        public Address Address { get { return m_Address; } set { m_Address = value; } }


    }
}
