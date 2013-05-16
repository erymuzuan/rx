using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.Sph.Windows.Models;

namespace Bespoke.Sph.Windows.ViewModels
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
