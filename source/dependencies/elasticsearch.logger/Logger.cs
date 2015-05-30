﻿using System;
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
        private readonly Func<string> m_url = () =>
            $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sys/log/{Guid.NewGuid()}";

        public Severity TraceSwitch { get; set; }

        public async Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;

            var content = GetJsonContent(entry);
            var url = m_url();

            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(url, content);
                Debug.WriteLine("{0}=>{1}",m_url, response.StatusCode);
            }
        }


        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;

            var content = GetJsonContent(entry);
            var url = m_url();

            using (var client = new HttpClient())
            {
                var response = client.PutAsync(url, content).Result;
                Debug.WriteLine("{0}=>{1}", m_url, response.StatusCode);
            }
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