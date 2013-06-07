using System.Linq;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class Rent : Entity
    {
        [XmlAttribute] 
        private decimal? m_accrued;

        public decimal? Accrued
        {
            get { return m_accrued; }
            set
            {
                m_accrued = value;
                RaisePropertyChanged();
            }
        }

        [XmlAttribute] 
        private decimal? m_accumulatedAccrued;

        public decimal? AccumulatedAccrued
        {
            get { return m_accumulatedAccrued; }
            set
            {
                m_accumulatedAccrued = value;
                RaisePropertyChanged();
            }
        }

        [XmlAttribute]
        public decimal? RentPaid
        {
            get 
            { 
                var rentPaid = this.PaymentDistributionCollection.Sum(a => a.Amount);
                return rentPaid;
            }
        }

        [XmlAttribute]
        public decimal? TotalPayment
        {
            get 
            { 
                var totalPayment = this.AccumulatedAccrued + this.Accrued;
                return totalPayment;
            }
        }
    }
}
