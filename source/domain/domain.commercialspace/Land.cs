using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{

    public partial class Land : SpatialEntity
    {
        private int m_landId;
        [XmlAttribute]
        public int LandId
        {
            get { return m_landId; }
            set
            {
                m_landId = value;
                RaisePropertyChanged();
            }
        }
    }
}