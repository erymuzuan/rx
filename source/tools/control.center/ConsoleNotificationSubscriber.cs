using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NamedPipeWrapper;
using Newtonsoft.Json;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.ControlCenter
{
    public class ConsoleNotificationSubscriber
    {
        private WebSocketServer m_appServer;
        private FileSystemWatcher m_fsw;
        // name pipe server listen to workers process and push them all to the browser via websocket
        private NamedPipeServer<string> m_messageListenerServer;

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
            if (started)
                StartNamePipeServer();

            return started;
        }


        private void StartNamePipeServer()
        {
            m_messageListenerServer = new NamedPipeServer<string>("rx.web.console");
            m_messageListenerServer.ClientConnected += delegate (NamedPipeConnection<string, string> conn)
            {
                Console.WriteLine("Client {0} is now connected!", conn.Id);
                conn.PushMessage("Welcome!");
            };

            m_messageListenerServer.ClientMessage += delegate (NamedPipeConnection<string, string> conn, string message)
            {
                SendMessage(message);
            };
            m_messageListenerServer.Start();

        }
        public bool Stop()
        {
            if (null != m_fsw)
            {
                m_fsw.Changed -= FswChanged;
                m_fsw.Dispose();
            }
            m_appServer.Stop();
            m_appServer.Dispose();
            m_messageListenerServer?.Stop();
            return true;
        }

        public void Listen()
        {
            // TODO : listen to name pipe for notification
        }


        protected void OnStop() { }

        void FswChanged(object sender, FileSystemEventArgs e)
        {
            //this.WriteInfo($"Detected changes in FileSystem \r\n{e.Name} has {e.ChangeType}");
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message = $"{e.ChangeType} in output {e.FullPath}", severity = "Info", outputFile = e.FullPath });
            SendMessage(json);
        }
        private void SendMessage(string json)
        {
            m_appServer?.GetAllSessions().ToList().ForEach(x =>
            {
                try
                {
                    x.Send(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
        private void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("Getting new message from {0} => {1}", session.SessionID, value);
            if (value.StartsWith("POST /deploy:"))
            {
                var outputs = value.Replace("POST /deploy:", "")
                    .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                var parent = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\");
                var projectPath = Path.GetFullPath(parent);

                Parallel.ForEach(outputs, f =>
                {
                    if (!File.Exists(f)) return;

                    var fileName = Path.GetFileName(f) ?? "";
                    if (fileName.StartsWith("ff")) return;

                    Console.WriteLine($"Copying {fileName} to subsribers");
                    File.Copy(f, $"{projectPath}\\subscribers\\{fileName}", true);

                    if (fileName.StartsWith("subscriber.trigger")) return;

                    Console.WriteLine($"Copying {fileName} to schedulers");
                    File.Copy(f, $"{projectPath}\\schedulers\\{fileName}", true);
                    Console.WriteLine($"Copying {fileName} to web\\bin");
                    File.Copy(f, $"{projectPath}\\web\\bin\\{fileName}", true);
                    Console.WriteLine($"Done copying {fileName}");
                });

            }

        }


    }
}
