using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{

    public partial class ContractTemplate : Entity
    {
        private int m_contractTemplateId;

        [XmlAttribute]
        public int ContractTemplateId
        {
            get { return m_contractTemplateId; }
            set
            {
                m_contractTemplateId = value;
                RaisePropertyChanged();
            }
        }
    }
}