using System.ComponentModel.Composition;
using Bespoke.Station.Windows.ViewModels.Dashboard;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views.Dashboards
{
    [Export(typeof(RadTileViewItem))]
    public partial class InventoryTileView 
    {
        public InventoryTileView()
        {
            InitializeComponent();
        }

      
    }
}
