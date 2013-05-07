using System.Xml.Serialization;

namespace Bespoke.Station.Domain
{
    public partial  class Setting : Entity
    {
        private int m_settingId;

        [XmlAttribute]
        public int SettingId
        {
            get { return m_settingId; }
            set
            {
                m_settingId = value;
                RaisePropertyChanged();
            }
        }
    }
}
