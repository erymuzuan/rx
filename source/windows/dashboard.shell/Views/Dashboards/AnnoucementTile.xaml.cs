using System.ComponentModel.Composition;
using Bespoke.Sph.Windows.ViewModels.Dashboard;

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
