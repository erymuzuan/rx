using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.ControlCenter
{
    public class WebConsoleServer
    {
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

        public bool Stop()
        {
            if (null != m_fsw)
            {
                m_fsw.Changed -= FswChanged;
                m_fsw.Dispose();
            }
            m_appServer.Stop();
            m_appServer.Dispose();
            return true;
        }

        void FswChanged(object sender, FileSystemEventArgs e)
        {
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message = $"{e.ChangeType} in output {e.FullPath}", severity = "Info", outputFile = e.FullPath });
            SendMessage(json);
        }

        public void SendMessage(string json)
        {
            m_appServer?.GetAllSessions().ToList().ForEach(x =>
            {
                try
                {
                    x?.Send(json);
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
