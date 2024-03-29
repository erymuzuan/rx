﻿using System;
using System.ComponentModel.Composition;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Extensions;
using Polly;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    [Export("Bespoke.Sph.Domain.ICancelledMessageRepository", typeof(ICancelledMessageRepository))]
    public class CancelledMessageRepository : ICancelledMessageRepository
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        public string Host { get; }

        private readonly HttpClient m_client;

        public readonly string DailyIndex =
            $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sla_{DateTime.Today:yyyyMMdd}";

        public bool Profiled { get; set; }
        public int HttpRequestRetryCount { get; set; } = 3;
        public int HttpRequestWaitTime { get; set; } = 200;
        public TimeSpan CheckMessageBreakDuration { get; set; } = TimeSpan.FromSeconds(30);
        public int CheckMessageAllowedExceptionsBeforeBreaking { get; set; } = 3;

        public WaitAlgorithm HttpRequestWaitAlgorithm { get; set; } = WaitAlgorithm.Exponential;
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global

        /// <summary>
        /// $"{Host}/{Index}/event"
        /// </summary>
        private string DailyTypeUri => $"{DailyIndex}/cancelledmessage";
        private static string AliasTypeUri => $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sla/cancelledmessage";

        public CancelledMessageRepository()
        {
            Host = ConfigurationManager.GetEnvironmentVariable("ElasticsearchMessageTrackingHost") ?? EsConfigurationManager.Host;
            m_client = new HttpClient { BaseAddress = new Uri(Host) };
        }


        private CircuitBreaker m_checkMessageCircuitBreaker;

        public async Task<bool> CheckMessageAsync(string messageId, string worker)
        {
            if (null == m_checkMessageCircuitBreaker)
                m_checkMessageCircuitBreaker = new CircuitBreaker(CheckMessageAllowedExceptionsBeforeBreaking, CheckMessageBreakDuration);

            if (m_checkMessageCircuitBreaker.IsOpen)
            {
                await m_checkMessageCircuitBreaker.WaitToCloseAsync();
            }

            var url = $"{AliasTypeUri}/_count";
            var query = $@"{{
   ""query"": {{
      ""bool"": {{
         ""must"": [
            {{
               ""term"": {{
                  ""messageId"": {{
                     ""value"": ""{messageId}""
                  }}
               }}
            }},
            {{
                ""term"": {{
                   ""worker"": {{
                      ""value"": ""{worker}""
                   }}
                }}
                
            }}
            
         ]
      }}
   }}
}}
";

            var pr = await Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(HttpRequestRetryCount, Wait)
                .ExecuteAndCaptureAsync(async () =>
                {
                    var response = await m_client.PostAsync(url, new StringContent(query));
                    var json = await response.ReadContentAsJsonAsync();
                    var total = json.SelectToken("$.count").Value<int>();
                    return total > 0;
                });
            return m_checkMessageCircuitBreaker.GetResult(pr);
        }

        TimeSpan Wait(int c)
        {
            switch (HttpRequestWaitAlgorithm)
            {
                case WaitAlgorithm.Linear:
                    return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime * c);
                case WaitAlgorithm.Exponential:
                    return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime * Math.Pow(2, c));
                case WaitAlgorithm.Constant:
                    return TimeSpan.FromMilliseconds(this.HttpRequestWaitTime);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        public async Task PutAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "").Replace(" ", "").Replace("-", "").Replace(".", "");
            var json = $@"{{
        ""messageId"": ""{messageId}"",
        ""worker"": ""{worker}""
}}";


            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(HttpRequestRetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    var r = await m_client.PostAsync($"{DailyTypeUri}/{id}", new StringContent(json));
                    r.EnsureSuccessStatusCode();
                });
        }

        public async Task RemoveAsync(string messageId, string worker)
        {
            var id = $"{messageId}{worker}".Replace("/", "").Replace("/", "").Replace(" ", "").Replace("-", "").Replace(".", "");

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(HttpRequestRetryCount, Wait)
                .ExecuteAsync(async () =>
                {
                    var r = await m_client.DeleteAsync($"{AliasTypeUri}/{id}");
                    r.EnsureSuccessStatusCode();
                });
        }


        private bool m_initialized;

        public async Task InitializeAsync()
        {
            if (m_initialized) return;

            // create index if not exist
            await m_client.PutAsync(DailyIndex, new StringContent(""));

            // create es mapping
            var content = new StringContent(Properties.Resources.CancelledMessageMapping);
            var response = await m_client.PutAsync($"{DailyIndex}/_mapping/cancelledmessage", content);
            response.EnsureSuccessStatusCode();

            m_initialized = true;
        }
    }
}