using System.ComponentModel.Composition;
using Bespoke.Station.Windows.ViewModels.Dashboard;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views.Dashboards
{
    //[Export(typeof(RadTileViewItem))]
    public partial class AnnoucementTile 
    {
        public AnnoucementTile()
        {
            InitializeComponent();
        }

        [Import]
        public AnnoucementTileViewModel ViewModel
        {
            get { return this.DataContext as AnnoucementTileViewModel; }
            set { this.DataContext = value; }
        }
    }
}
