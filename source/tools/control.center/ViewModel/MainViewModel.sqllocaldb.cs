using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using Bespoke.Sph.ControlCenter.Properties;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel 
    {
        public RelayCommand StartSqlServiceCommand { get; set; }
        public RelayCommand StopSqlServiceCommand { get; set; }

        public void SetupSqlLocaldbCommand()
        {
            StartSqlServiceCommand = new RelayCommand(StartSqlService, () => !SqlServiceStarted);
            StopSqlServiceCommand = new RelayCommand(StopSqlService, () => SqlServiceStarted);

        }

        private void CheckSqlServer()
        {
            try
            {
                using (var conn = new SqlConnection($"Data Source=(localdb)\\{this.Settings.SqlLocalDbName};Initial Catalog={this.Settings.ApplicationName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"))
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM [Sph].[UserProfile]", conn))
                {
                    conn.Open();
                    var count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {

                        SqlServiceStarted = true;
                        SqlServiceStatus = "Running";
                    }
                }

            }
            catch (SqlException)
            {

            }
        }

        public async void StartSqlService()
        {
            if (string.IsNullOrEmpty(this.Settings.SqlLocalDbName))
            {
                MessageBox.Show("Instance name cannot be empty", "SPH Control Panel");
                return;
            }

            Log("SqlLocalDb...[STARTING]");
            try
            {

                var workerInfo = new ProcessStartInfo
                {
                    FileName = "SqlLocalDB.exe",
                    Arguments = $"start \"{this.Settings.SqlLocalDbName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using (var p = Process.Start(workerInfo))
                {
                    if (null == p) throw new InvalidOperationException("Cannot start sql");
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.OutputDataReceived += OnDataReceived;
                    p.ErrorDataReceived += OnDataReceived;
                    p.WaitForExit();
                }

                SqlServiceStarted = true;
                SqlServiceStatus = "Running";
                Log("SqlLocalDb... [STARTED]");
                this.IsSetup = await this.FindOutSetupAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void StopSqlService()
        {
            Log("SqlLocalDb...[STOPPING]");
            try
            {

                var workerInfo = new ProcessStartInfo
                {
                    FileName = "SqlLocalDB.exe",
                    Arguments = $"stop \"{this.Settings.SqlLocalDbName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using (var p = Process.Start(workerInfo))
                {
                    if (null == p) throw new InvalidOperationException("Cannot start SQL");
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.OutputDataReceived += OnDataReceived;
                    p.ErrorDataReceived += OnDataReceived;
                    p.WaitForExit();
                }

                SqlServiceStarted = false;
                SqlServiceStatus = "Stopped";
                Log("SqlLocalDb... [STOPPED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

    }
}
