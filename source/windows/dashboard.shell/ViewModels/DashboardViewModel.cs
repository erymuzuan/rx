using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Models;
using System.ComponentModel.Composition;

namespace Bespoke.Station.Windows.ViewModels
{
    [Export]
    public partial class DashboardViewModel : StationViewModelBase<DashboardTask>
    {
    }
}