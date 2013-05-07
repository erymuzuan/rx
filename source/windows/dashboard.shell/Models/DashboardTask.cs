using Bespoke.Cycling.Domain;

namespace Bespoke.Station.Windows.Models
{
    public class DashboardTask:DomainObject
    {
        private string m_name;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                RaisePropertyChanged();
            }
        }

    }
}
