using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public partial class Organization : Entity
    {
        private int m_organizationId;

        [Key]
        [XmlAttribute]
        public int OrganizationId
        {
            get { return m_organizationId; }
            set
            {
                m_organizationId = value;
                OnPropertyChanged();
            }
        }

         
    }
}