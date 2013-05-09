using System.ComponentModel.Composition;
using Bespoke.Sph.Windows.ViewModels.Dashboard;

namespace Bespoke.Sph.Windows.Views.Dashboards
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
