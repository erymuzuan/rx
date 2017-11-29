using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainViewWithRabbitMq
    {
        public MainViewWithRabbitMq()
        {
            InitializeComponent();
        }

        private void Navigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void NavigateApp(object sender, RequestNavigateEventArgs e)
        {
            if (!(this.DataContext is MainViewModel)) throw new InvalidOperationException("The DataContext is not MainViewModel");

            Process.Start(new ProcessStartInfo(ConfigurationManager.BaseUrl));
            e.Handled = true;
        }

        private void OpenRepositoryManagement(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                 ---------------------------
Microsoft SQL Server Management Studio
---------------------------
Usage:

ssms.exe [-S server_name[\instance_name]] [-d database] [-U user] [-P password] [-E] [-nosplash] [file_name[, file_name]*] [-log [file_name]?] [-?]

[-S  The name of the SQL Server instance to connect to]
[-d  The name of the SQL Server database to connect to]
[-E]  Use Windows Authentication to login to SQL Server
[-U  The name of the SQL Server login to connect with]
[-P  The password associated with the login]
[-nosplash] Suppress splash screen
[file_name[, file_name]*] Names of files to load
[-log [file_name]?] Logs SQL Server Management Studio activity to the specified file for troubleshooting
[-?]  Displays this usage information
---------------------------
OK   
---------------------------

 */
                Process.Start("ssms");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail to start SSMS \r\n" + ex.Message, "Rx", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
