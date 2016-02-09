using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticSearchLogger
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {

        public Severity TraceSwitch { get; set; }

        public async Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;

            if (string.IsNullOrWhiteSpace(entry.Id))
                entry.Id = Strings.GenerateId();

            var content = GetJsonContent(entry);
            var url = $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchSystemIndex}/log/{entry.Id}"; 

            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("{0}=>{1}",url, response.StatusCode);
            }
        }


        public void Log(LogEntry entry)
        {
            this.LogAsync(entry).ContinueWith(_=> {});
        }
        private static StringContent GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            var content = new StringContent(json);
            return content;
        }
    }
}