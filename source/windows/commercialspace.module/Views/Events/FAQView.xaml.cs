using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "FAQ", Order = 8)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class FaqView
    {
        public FaqView()
        {
            InitializeComponent();
        }

        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set { this.DataContext = value; }
        }


        private void AddFaqSectionClicked(object sender, RoutedEventArgs e)
        {
            var sectionWindow = new AddFaqSectionWindow { ViewModel = ViewModel };
            sectionWindow.Show(); 
        }

   
      
    }
}
