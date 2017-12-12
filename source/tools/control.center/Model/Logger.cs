using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.CircuitBreaker;

namespace Bespoke.Sph.ControlCenter.Model
{

    public class Logger
    {
        private readonly CircuitBreakerPolicy m_circuit;
        public Severity TraceSwitch { get; set; }

        public Logger()
        {

            m_circuit = Policy.Handle<Exception>()
                 .CircuitBreaker(3, TimeSpan.FromSeconds(5));
        }

        private void SendMessage(string json, Severity severity)
        {
         
            Action send = () =>
            {
                //TODO : send the message to web console. shared memory/name pipes
            };

            try
            {
                Policy.Handle<Exception>()
                    .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(c * 500))
                    .Execute(send);
            }
            catch
            {
                //ignore
            }
        }

        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            try
            {
                if ((int)entry.Severity < (int)this.TraceSwitch)
                    return;
                var json = GetJsonContent(entry);
                this.QueueUserWorkItem(SendMessage, json, entry.Severity);
            }
            catch
            {
                // ignored
            }
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