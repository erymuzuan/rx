using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Bespoke.Sph.ControlCenter.ViewModel;
using Newtonsoft.Json;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.ControlCenter
{
    public class WebConsoleServer
    {
        private const string WebConsoleLogger = "web.console.logger";
        private static WebConsoleServer m_instance;

        public static WebConsoleServer Default => m_instance ?? (m_instance = new WebConsoleServer());

        private WebConsoleServer()
        {

        }
        private WebSocketServer m_appServer;
        private FileSystemWatcher m_fsw;

        public bool Start(MainViewModel mainViewModel, int port = 50230)
        {
            m_mainViewModel = mainViewModel;
            var output = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "..\\output\\");
            m_fsw = new FileSystemWatcher(output)
            {
                EnableRaisingEvents = true
            };
            m_fsw.Changed += FswChanged;
            m_fsw.Deleted += FswChanged;
            m_fsw.Created += FswChanged;
            m_appServer = new WebSocketServer();
            if (!m_appServer.Setup(port))
            {
                return false;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;
            var started = m_appServer.Start();

            return started;
        }


        
        private MainViewModel m_mainViewModel;

        public void StartConsume(MainViewModel mainViewModel)
        {
            m_mainViewModel = mainViewModel;
            var settings = mainViewModel.Settings;
          
            // TODO : connect to shared memory /name pipes what ever

        }
        public void StopConsume()
        {

            Console.WriteLine("!!Stoping : {0}", WebConsoleLogger);

            this.Stop();

        }



        public bool Stop()
        {
            if (null != m_fsw)
            {
                m_fsw.Changed -= FswChanged;
                m_fsw.Dispose();
            }
            m_appServer?.Stop();
            m_appServer?.Dispose();

            return true;
        }

        private void FswChanged(object sender, FileSystemEventArgs e)
        {
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message = $"{e.ChangeType} in output {e.FullPath}", severity = "Info", outputFile = e.FullPath });
            SendMessage(json);
            try
            {
                if (!this.CreatedFileCollection.Contains(e.FullPath))
                    this.CreatedFileCollection.Add(e.FullPath);
            }
            catch (IndexOutOfRangeException)
            {

            }
        }
        private void WriteMessage(string message)
        {
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message, severity = "Info" });
            SendMessage(json);
        }

        public void SendMessage(string json)
        {
            this.QueueUserWorkItem(m =>
            {
                m_appServer?.GetAllSessions().Where(x => null != x).ToList().ForEach(x =>
                {
                    try
                    {
                        x?.Send(m);
                    }
                    catch
                    {
                        // ignored
                    }

                });
            }, json);
        }

        private async void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine($"Getting new message from {session.SessionID} => {value}");
            if (value.StartsWith("POST /bring-to-view:"))
            {
                m_mainViewModel.Post(() =>
                {
                    var wdw = Application.Current.MainWindow;
                    wdw.WindowState = WindowState.Normal;
                    wdw.Activate();
                    if (!wdw.Topmost)
                    {
                        wdw.Topmost = true;
                        wdw.Topmost = false;
                    }
                    wdw.Focus();
                });
                return;
            }
            if (!value.StartsWith("POST /deploy:")) return;

            // stop the workers
            m_mainViewModel.StopSphWorkerCommand.Execute(null);
            await Task.Delay(2500);

            var outputs = value.Replace("POST /deploy:", "")
                .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            Parallel.ForEach(outputs, DeployOutput);
            await WarmupWebServerAsync();
            try
            {
                this.CreatedFileCollection.Clear();
            }
            catch (IndexOutOfRangeException)
            {

            }

            // restart the workers
            while (!m_mainViewModel.StartSphWorkerCommand.CanExecute(null))
            {
                await Task.Delay(500);
            }
            m_mainViewModel.StartSphWorkerCommand.Execute(null);


        }

        public void DeployOutput(string f)
        {
            m_mainViewModel?.StartBusy($"Deploying {Path.GetFileName(f)} ..");
            if (!File.Exists(f)) return;

            var fileName = Path.GetFileName(f) ?? "";
            if (fileName.StartsWith("ff")) return;

            WriteMessage($"Copying {fileName} to subsribers");
            File.Copy(f, $"{ConfigurationManager.SubscriberPath}\\{fileName}", true);

            if (fileName.StartsWith("subscriber.trigger")) return;

            WriteMessage($"Copying {fileName} to schedulers");
            File.Copy(f, $"{ConfigurationManager.SchedulerPath}\\{fileName}", true);
            WriteMessage($"Copying {fileName} to web\\bin");
            File.Copy(f, $"{ConfigurationManager.WebPath}\\bin\\{fileName}", true);
            WriteMessage($"Done copying {fileName}");
            m_mainViewModel?.StopBusy();

        }

        public async Task WarmupWebServerAsync()
        {
            WriteMessage("Warming up the webserver");
            m_mainViewModel?.StartBusy("Warming up the webserver");
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.BaseUrl) })
                {
                    await client.GetAsync("/");
                    await client.GetAsync("/");
                }
            }
            catch (TaskCanceledException e)
            {
                WriteMessage(nameof(TaskCanceledException));
                WriteMessage(e.Message);
            }
            finally
            {
                WriteMessage("Done issue request to the web server");
                m_mainViewModel?.StopBusy();

            }
        }

        public ObservableCollection<string> CreatedFileCollection { get; } = new ObservableCollection<string>();
    }
}
