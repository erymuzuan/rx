using System.ComponentModel;

namespace Bespoke.Sph.ControlCenter.Helpers
{
    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);

    public class PropertyChangingEventArgs : CancelEventArgs
    {
        private string  m_propertyName;
        private object m_value;

        public PropertyChangingEventArgs()
        {

        }

        public PropertyChangingEventArgs(string propertyName, object newValue)
        {
            m_value = newValue;
            m_propertyName = propertyName;
        }

        public object NewValue
        {
            get { return m_value; }
            set { m_value = value; }
        }


        public string  PropertyName
        {
            get { return m_propertyName; }
            set { m_propertyName = value; }
        }

    }
}
