using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class Contract : Entity
    {
        private int m_contractId;

        [XmlAttribute]
        public int ContractId
        {
            get { return m_contractId; }
            set
            {
                m_contractId = value;
                RaisePropertyChanged();
            }
        }
    }
}