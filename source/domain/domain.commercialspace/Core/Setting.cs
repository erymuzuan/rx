using System.Xml.Serialization;

namespace Bespoke.CommercialSpace.Domain
{
    public partial class Setting : Entity
    {
        private int m_settingId;
        private string m_key;
        private string m_value;

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

        public string Key
        {
            get { return m_key; }
            set
            {
                m_key = value;
                RaisePropertyChanged();
            }
        }

        public string Value
        {
            get { return m_value; }
            set
            {
                m_value = value;
                RaisePropertyChanged();
            }
        }
    }
}
