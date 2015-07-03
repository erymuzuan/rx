using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Mindscape.Raygun4Net;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RayGunLoggers
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        private readonly RaygunClient m_client;
        private readonly List<string> m_tags = new List<string>();
        public Logger()
        {
            m_tags.Add(ConfigurationManager.ApplicationName);
            var version = "";
            string file = $"{ConfigurationManager.WebPath}\\..\\version.json";
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var build = json.SelectToken("$.build").Value<int>();
                version = build.ToString();
            }
            m_client = new RaygunClient("imHU3x8eZamg84BwYekfMQ==")
            {
                ApplicationVersion = version
            };
        }

        public Severity TraceSwitch { get; set; }

        public Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return Task.FromResult(0);
            if (null == entry.Exception) return Task.FromResult(0);

            var data = AddData(entry);
            m_client.SendInBackground(entry.Exception, m_tags, data);

            return Task.FromResult(0);
        }


        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;
            if (null == entry.Exception) return;

            var data = AddData(entry);

            m_client.Send(entry.Exception, m_tags, data);
        }

        private static Dictionary<string, object> AddData(LogEntry entry)
        {
            var data = new Dictionary<string, object>
            {
                {"Message", entry.Message},
                {"CallerFilePath", entry.CallerFilePath},
                {"CallerLineNumber", entry.CallerLineNumber},
                {"CallerMemberName", entry.CallerMemberName},
                {"Operation", entry.Operation},
                {"Severity", entry.Severity},
                {"Computer", entry.Computer},
                {"Details", entry.Details}
            };
            foreach (var o in entry.OtherInfo)
            {
                if (data.ContainsKey(o.Key))
                    data[o.Key] = o.Value;
                else
                    data.Add(o.Key, o.Value);
            }
            return data;
        }
    }
}