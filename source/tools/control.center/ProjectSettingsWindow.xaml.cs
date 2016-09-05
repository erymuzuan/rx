using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class ProjectSettingsWindow
    {
        public ProjectSettingsWindow()
        {
            InitializeComponent();
            this.Loaded += ProjectSettingsUserControlLoaded;
        }
        private readonly IList<string> m_sqlLocaldbInstances = new List<string>();
        async void ProjectSettingsUserControlLoaded(object sender, RoutedEventArgs e)
        {
            var list = await LoadSqlLocaldbInstances();
            m_sqlLocaldbInstances.Clear();
            list.ToList().ForEach(x => m_sqlLocaldbInstances.Add(x));
            LoadEnvironments();
        }

        private void LoadEnvironments()
        {
            var variables = System.Environment.GetEnvironmentVariables();

            var sb = new StringBuilder();
            foreach (var v in variables.Keys)
            {
                var key = $"{v}";
                if (key.StartsWith("RX_"))
                    sb.AppendLine($"{v} = {variables[v]}");
            }

            environmentValues.Text = sb.ToString();
        }

        private Task<string[]> LoadSqlLocaldbInstances()
        {
            if (m_sqlLocaldbInstances.Count > 0)
                m_sqlLocaldbInstances.Clear();

            var tcs = new TaskCompletionSource<string[]>();
            this.QueueUserWorkItem(() =>
            {
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
                    var list = new ConcurrentBag<string>();
                    p.OutputDataReceived += (s, e) =>
                    {
                        if (!string.IsNullOrWhiteSpace(e.Data))
                            list.Add(e.Data);
                    };
                    p.WaitForExit();
                    tcs.SetResult(list.ToArray());
                }
            });

            return tcs.Task;
        }


        private void SaveSetting(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            vm.SaveSettingsCommand.Execute(null);
            this.DialogResult = true;
            this.Close();
        }
    }
}
