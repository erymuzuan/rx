using System.ComponentModel.Composition;
using Bespoke.Station.Windows.ViewModels.Dashboard;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views.Dashboards
{
    //[Export(typeof(RadTileViewItem))]
    public partial class TodoTile 
    {
        public TodoTile()
        {
            InitializeComponent();
        }

        [Import]
        public TodoTileViewModel ViewModel
        {
            get { return this.DataContext as TodoTileViewModel; }
            set { this.DataContext = value; }
        }
    }
}
