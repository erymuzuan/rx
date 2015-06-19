using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Messaging
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public Severity TraceSwitch { get; set; }
        private void SendMessage(string json)
        {
            WebSocketNotificationService.Instance.WriteRaw(json);
        }

        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }



        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch)
                return;

            var json = GetJsonContent(entry);
            this.SendMessage(json);
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
    }
}
