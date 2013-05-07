using System;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class StageDialog
    {
        public StageDialog()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        protected StageViewModel ViewModel
        {
            get { return this.DataContext as StageViewModel; }
            set {this.DataContext = value; }
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void OpenPopupClick(object sender, RoutedEventArgs e)
        {
            var name = ((Button) sender).Tag as string;
           // var options = new HtmlPopupWindowOptions {Left = 0, Top = 0, Width = 800, Height = 600};

        //    if (HtmlPage.IsPopupWindowAllowed)
        //        HtmlPage.PopupWindow(new Uri("http://www.angrychains.com/route/details/" + name), "_blank", options);
        //
        }


    }
}

