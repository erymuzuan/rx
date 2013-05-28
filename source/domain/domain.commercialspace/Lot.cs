using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class Lot : DomainObject
    {
        private string m_planStoreId;
        private string m_fillColor;
        private double m_fillOpacity;
        private string m_usage;

        [XmlAttribute]
        public string Usage
        {
            get { return m_usage; }
            set
            {
                m_usage = value;
                RaisePropertyChanged();
            }
        }

        [XmlAttribute]
        public double FillOpacity
        {
            get { return m_fillOpacity; }
            set
            {
                m_fillOpacity = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string FillColor
        {
            get { return m_fillColor; }
            set
            {
                m_fillColor = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string PlanStoreId
        {
            get { return m_planStoreId; }
            set
            {
                m_planStoreId = value;
                RaisePropertyChanged();
            }
        }
    }
}
