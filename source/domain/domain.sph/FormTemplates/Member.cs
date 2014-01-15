using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class Member : DomainObject
    {
        [XmlAttribute]
        public string FullName { get; set; }

        public override string ToString()
        {
            return string.Format("{0}->{1}:{2}", this.Name, this.FullName, this.TypeName);
        }
    }
}