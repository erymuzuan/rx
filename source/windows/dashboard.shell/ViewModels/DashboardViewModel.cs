using Bespoke.Sph.Windows.Infrastructure;
using Bespoke.Sph.Windows.Models;
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Windows.ViewModels
{
    [Export]
    public partial class DashboardViewModel : SphViewModelBase<DashboardTask>
    {
    }
}