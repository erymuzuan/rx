using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace web.console.logger
{
    [Obsolete("Figure out something without RabbitMq, like shared memory/memory map files/name pipes etc")]
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public const int NON_PERSISTENT_DELIVERY_MODE = 2;
        public Severity TraceSwitch { get; set; }


        private void SendMessage(string json, Severity severity)
        {
            //TODO: Figure out something without RabbitMq, like shared memory/memory map files/name pipes etc"
        }

        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }


        public void Log(LogEntry entry)
        {
            if ((int) entry.Severity < (int) this.TraceSwitch)
                return;

            var json = GetJsonContent(entry);
            this.SendMessage(json, entry.Severity);
        }

        private static string GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore};
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            return json;
        }
    }
}