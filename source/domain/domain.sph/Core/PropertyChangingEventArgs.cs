using System.ComponentModel;

namespace Bespoke.Sph.Domain
{
    public delegate void PropertyChangingEventHandler(object sender, PropertyChangingEventArgs e);

    public class PropertyChangingEventArgs : CancelEventArgs
    {
        public PropertyChangingEventArgs()
        {

        }

        public PropertyChangingEventArgs(string propertyName, object newValue)
        {
            NewValue = newValue;
            PropertyName = propertyName;
        }

        public object NewValue { get; set; }
        public string PropertyName { get; set; }
    }
}
