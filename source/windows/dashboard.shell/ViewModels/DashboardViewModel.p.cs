using Bespoke.Cycling.Domain;
using Bespoke.Station.Windows.Models;

namespace Bespoke.Station.Windows.ViewModels
{
    public partial class DashboardViewModel
    {
        private readonly ObjectCollection<DashboardTask> m_dashboardCollection = new ObjectCollection<DashboardTask>();

        public ObjectCollection<DashboardTask> DashboardTaskCollection
        {
            get { return m_dashboardCollection; }
        }
    }
}
