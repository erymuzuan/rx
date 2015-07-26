using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bespoke.Sph.ControlCenter.ViewModel;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
using RabbitMQ.Client;
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

        public bool Start(int port = 50230)
        {
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


        private IModel m_channel;
        private IConnection m_connection;
        private TaskBasicConsumer m_consumer;
        private MainViewModel m_mainViewModel;

        public void StartConsume(MainViewModel mainViewModel)
        {
            m_mainViewModel = mainViewModel;
            var settings = mainViewModel.Settings;
            const bool NO_ACK = true;
            const string EXCHANGE_NAME = "sph.topic";
            var factory = new ConnectionFactory
            {
                UserName = settings.RabbitMqUserName ?? "guest",
                Password = settings.RabbitMqPassword ?? "guest",
                VirtualHost = settings.ApplicationName,
                Port = settings.RabbitMqPort ?? 5672,
                HostName = settings.RabbitMqHost ?? "localhost"

            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();
            var qd = m_channel.QueueDeclare(WebConsoleLogger, false, true, true, null);
            Console.WriteLine(qd);
            m_channel.QueueBind(WebConsoleLogger, EXCHANGE_NAME, "logger.#", null);
            m_channel.BasicQos(0, 10, false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(WebConsoleLogger, NO_ACK, m_consumer);

        }
        public void StopConsume()
        {
            Console.WriteLine("!!Stoping : {0}", WebConsoleLogger);

            if (null != m_consumer)
                m_consumer.Received -= Received;

            m_channel?.Close();
            m_channel?.Dispose();
            m_channel = null;

            m_connection?.Dispose();
            m_connection = null;
        }

        private void Received(object sender, ReceivedMessageArgs e)
        {
            var json = Encoding.UTF8.GetString(e.Body);
            SendMessage(json);
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

        void FswChanged(object sender, FileSystemEventArgs e)
        {
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message = $"{e.ChangeType} in output {e.FullPath}", severity = "Info", outputFile = e.FullPath });
            SendMessage(json);
        }
        void WriteMessage(string message)
        {
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message, severity = "Info" });
            SendMessage(json);
        }

        public void SendMessage(string json)
        {
            m_appServer?.GetAllSessions().Where(x => null != x).ToList().ForEach(x =>
            {
                this.QueueUserWorkItem(m =>
                {
                    try
                    {
                        x?.Send(json);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }, json);
            });
        }

        private async void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("Getting new message from {0} => {1}", session.SessionID, value);
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

            var parent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\");
            var projectPath = Path.GetFullPath(parent);

            Parallel.ForEach(outputs, f =>
            {
                if (!File.Exists(f)) return;

                var fileName = Path.GetFileName(f) ?? "";
                if (fileName.StartsWith("ff")) return;

                WriteMessage($"Copying {fileName} to subsribers");
                File.Copy(f, $"{projectPath}\\subscribers\\{fileName}", true);

                if (fileName.StartsWith("subscriber.trigger")) return;

                WriteMessage($"Copying {fileName} to schedulers");
                File.Copy(f, $"{projectPath}\\schedulers\\{fileName}", true);
                WriteMessage($"Copying {fileName} to web\\bin");
                File.Copy(f, $"{projectPath}\\web\\bin\\{fileName}", true);
                WriteMessage($"Done copying {fileName}");
            });

            // restart the workers
            while (!m_mainViewModel.StartSphWorkerCommand.CanExecute(null))
            {
                await Task.Delay(500);
            }
            m_mainViewModel.StartSphWorkerCommand.Execute(null);


        }


    }
}
