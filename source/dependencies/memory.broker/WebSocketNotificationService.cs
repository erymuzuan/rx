using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SuperSocket.WebSocket;
using INotificationService = Bespoke.Sph.SubscribersInfrastructure.INotificationService;

namespace Bespoke.Sph.Messaging
{
    class WebSocketNotificationService : INotificationService
    {
        private WebSocketServer m_appServer;
        public bool Start(int port = 50230)
        {
            m_appServer = new WebSocketServer();
            if (!m_appServer.Setup(port))
            {
                return false;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;
            return m_appServer.Start();
        }

        private void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine(value);
        }

        public bool Stop()
        {
            m_appServer.Stop();
            m_appServer.Dispose();
            return true;
        }
        private static string GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            return json;
        }
        public void WriteInfo(string message)
        {
            var log = new LogEntry { Message = message, Severity = Severity.Info };
            var json = GetJsonContent(log);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
        }
        public void WriteVerbose(string message)
        {
            var log = new LogEntry { Message = message, Severity = Severity.Verbose };
            var json = GetJsonContent(log);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
        }
        public void WriteWarning(string message)
        {
            var log = new LogEntry { Message = message, Severity = Severity.Warning };
            var json = GetJsonContent(log);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
        }
        public void Write(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var log = new LogEntry { Message = message, Severity = Severity.Info };
            var json = GetJsonContent(log);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
        }

        public void WriteError(string format, params object[] args)
        {
            var message = string.Format(format, args);
            var log = new LogEntry { Message = message, Severity = Severity.Error };
            var json = GetJsonContent(log);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
        }

        public void WriteError(Exception e, string message)
        {
            var log = new LogEntry(e);
            var json = GetJsonContent(log);
            WriteError(message);
            m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));

        }
    }
}
