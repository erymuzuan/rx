using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.Messaging
{
    internal class WebSocketNotificationService : ILogger
    {
        private static WebSocketNotificationService m_instance;
        private static readonly object m_lock = new object();

        private WebSocketNotificationService()
        {
        }

        public static WebSocketNotificationService Instance
        {
            get
            {
                lock (m_lock)
                {
                    return m_instance ?? (m_instance = new WebSocketNotificationService());
                }
            }
        }
        private WebSocketServer m_appServer;
        private FileSystemWatcher m_fsw;

        public bool Start(int port)
        {
            m_fsw = new FileSystemWatcher(ConfigurationManager.CompilerOutputPath)
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
            return m_appServer.Start();

        }
        void FswChanged(object sender, FileSystemEventArgs e)
        {
            this.WriteInfo($"Detected changes in FileSystem \r\n{e.Name} has {e.ChangeType}");
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message = $"{e.ChangeType} in output {e.FullPath}", severity = "Info", outputFile = e.FullPath });
            SendMessage(json);
        }

        private void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(value);
            if (value.StartsWith("POST /deploy:"))
            {
                var outputs = value.Replace("POST /deploy:", "")
                    .Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                Parallel.ForEach(outputs, f =>
                {
                    if (!File.Exists(f)) return;

                    var fileName = Path.GetFileName(f) ?? "";
                    if (fileName.StartsWith("ff")) return;

                    this.WriteVerbose($"Copying {fileName} to subsribers");
                    File.Copy(f, $"{ConfigurationManager.SubscriberPath}\\{fileName}", true);

                    if (fileName.StartsWith("subscriber.trigger")) return;

                    this.WriteVerbose($"Copying {fileName} to schedulers");
                    File.Copy(f, $"{ConfigurationManager.SchedulerPath}\\{fileName}", true);
                    this.WriteVerbose($"Copying {fileName} to web\\bin");
                    File.Copy(f, $"{ConfigurationManager.WebPath}\\bin\\{fileName}", true);
                    this.WriteVerbose($"Done copying {fileName}");
                });

            }
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
        
        
        private static string GetJsonContent(LogEntry entry)
        {
            entry.Time = DateTime.Now;
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            return json;
        }
 
    
    
        private void SendMessage(LogEntry log)
        {
            var json = GetJsonContent(log);
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

     
       
        public Task LogAsync(LogEntry entry)
        {
            SendMessage(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            SendMessage(entry);
        }
    }
}
