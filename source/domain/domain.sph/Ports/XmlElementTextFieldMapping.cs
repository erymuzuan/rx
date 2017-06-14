using System.Linq;
using System.Xml.Linq;

namespace Bespoke.Sph.Domain
{
    public class XmlElementTextFieldMapping : TextFieldMapping
    {
        private readonly XElement m_element;

        public XmlElementTextFieldMapping(XElement element)
        {
            m_element = element;
        }

        public XmlElementTextFieldMapping()
        {

        }
        public override Member GenerateMember()
        {
            if (m_element.HasElements || m_element.HasAttributes)
            {
                var complex = new ComplexMember
                {
                    Name = this.Name,
                    TypeName = this.Name,
                    AllowMultiple = this.AllowMultiple
                };
                var children = from f in this.FieldMappingCollection
                               select f.GenerateMember();
                complex.MemberCollection.AddRange(children);
                return complex;
            }
            return base.GenerateMember();
        }
    }
}