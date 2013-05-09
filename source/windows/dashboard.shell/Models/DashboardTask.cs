using System.ComponentModel;
using System.Runtime.CompilerServices;
using Bespoke.Sph.Windows.Annotations;

namespace Bespoke.Sph.Windows.Models
{
    public class DashboardTask: INotifyPropertyChanged
    {
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
