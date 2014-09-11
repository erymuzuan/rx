using System.Xml.Serialization;

namespace Bespoke.Sph.Domain
{
    public class SpatialStore : SpatialEntity
    {
        private string m_type;
        private string m_tag;
        private string m_searchText;
        private string m_storeId;

        [XmlAttribute]
        public string StoreId
        {
            get { return m_storeId; }
            set
            {
                m_storeId = value;
                RaisePropertyChanged();
            }
        }

        public string SearchText
        {
            get { return m_searchText; }
            set
            {
                m_searchText = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string Tag
        {
            get { return m_tag; }
            set
            {
                m_tag = value;
                RaisePropertyChanged();
            }
        }
        [XmlAttribute]
        public string Type
        {
            get { return m_type; }
            set
            {
                m_type = value;
                RaisePropertyChanged();
            }
        }
    
    }
}
