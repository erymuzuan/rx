using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class ProjectSettingsUserControl
    {
        public ProjectSettingsUserControl()
        {
            InitializeComponent();
            this.Loaded += ProjectSettingsUserControlLoaded;
        }

        void ProjectSettingsUserControlLoaded(object sender, RoutedEventArgs e)
        {
            if (sqlIntancesCombobox.Items.Count > 0) 
                sqlIntancesCombobox.Items.Clear();

            var workerInfo = new ProcessStartInfo
            {
                FileName = "SqlLocalDB.exe",
                Arguments = "i",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            using (var p = Process.Start(workerInfo))
            {
                if (null == p)
                {
                    MessageBox.Show("We cannot find SqlLocalDb in your computer");
                    return;
                }
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.OutputDataReceived += OnDataReceived;
                p.WaitForExit();
            }

        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.Data))
            {
                Delegate insert = new Action<string>(s => sqlIntancesCombobox.Items.Add(e.Data));
                this.Dispatcher.BeginInvoke(insert, DispatcherPriority.Normal, e.Data);
            }
        }

        private void SaveSetting(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel) this.DataContext;
            vm.SaveSettingsCommand.Execute(null);
        }
    }
}
