using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class Contract : Entity
    {
        private int m_contractId;
        private string m_status;

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

        [XmlAttribute]
        public string Status
        {
            get { return m_status; }
            set { m_status = value;
            RaisePropertyChanged();}
        }
    }
}