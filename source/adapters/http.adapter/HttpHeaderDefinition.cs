using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpHeaderDefinition : DomainObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        private bool m_canOverride;
        private Field m_field;
        private string m_defaultValue;
        private string m_originalValue;

        public string OriginalValue
        {
            get { return m_originalValue; }
            set
            {
                m_originalValue = value;
                RaisePropertyChanged("OriginalValue");
            }
        }

        public string DefaultValue
        {
            get { return m_defaultValue; }
            set
            {
                m_defaultValue = value;
                RaisePropertyChanged();
            }
        }

        public Field Field
        {
            get { return m_field; }
            set
            {
                m_field = value;
                RaisePropertyChanged();
            }
        }

        public bool CanOverride
        {
            get { return m_canOverride; }
            set
            {
                m_canOverride = value;
                RaisePropertyChanged();
            }
        }
    }
}