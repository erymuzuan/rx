using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class RentalApplication : Entity
    {
        private int m_rentalApplicationId;
        private string m_registrationNo;

        [XmlAttribute]
        public int RentalApplicationId
        {
            get { return m_rentalApplicationId; }
            set
            {
                m_rentalApplicationId = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string RegistrationNo
        {
            get { return m_registrationNo; }
            set
            {
                m_registrationNo = value;
                RaisePropertyChanged();
            }
        }
    }
}
