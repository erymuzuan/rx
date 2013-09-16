using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class CustomListValue : DomainObject
    {
        private readonly ObjectCollection<CustomFieldValue> m_customFieldValueCollection = new ObjectCollection<CustomFieldValue>();
        [XmlIgnore]
        public ObjectCollection<CustomFieldValue> CustomFieldCollection
        {
            get { return m_customFieldValueCollection; }
        }
    }
}
