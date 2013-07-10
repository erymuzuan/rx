using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace permissions.editor
{
    internal class Permission : INotifyPropertyChanged
    {
        private string m_name;
        private string m_description;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return m_description; }
            set
            {
                m_description = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}