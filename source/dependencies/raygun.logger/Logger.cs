using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Bespoke.Sph.Domain;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Builders;
using Mindscape.Raygun4Net.Messages;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RayGunLoggers
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        private readonly List<string> m_tags = new List<string>();
        private readonly string m_version;
        public Logger()
        {
            m_tags.Add(ConfigurationManager.ApplicationName);
            string file = $"{ConfigurationManager.WebPath}\\..\\version.json";
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var build = json.SelectToken("$.build").Value<int>();
                m_version = build.ToString();
            }
        }

        public Severity TraceSwitch { get; set; }

        public async Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;
            if (null == entry.Exception) return;

            var data = AddData(entry);
            var currentRequestMessage = this.BuildRequestMessage();
            try
            {
                m_currentRequestMessage = currentRequestMessage;
                await this.StripAndSend(entry.Exception, m_tags, data);
            }
            catch (Exception)
            {
                // ignored
            }
        }


        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;
            if (null == entry.Exception) return;

            var data = AddData(entry);

            var currentRequestMessage = this.BuildRequestMessage();
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    m_currentRequestMessage = currentRequestMessage;
                    this.StripAndSend(entry.Exception, m_tags, data)
                    .ContinueWith(_ =>
                    {
                        Console.WriteLine(@"done");
                    });
                }
                catch (Exception)
                {
                    // ignored
                }
            });
        }

        private RaygunRequestMessage BuildRequestMessage()
        {
            RaygunRequestMessage requestMessage = null;
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                HttpRequest request = null;
                try
                {
                    request = context.Request;
                }
                catch (HttpException ex)
                {
                    Trace.WriteLine("Error retrieving HttpRequest {0}", ex.Message);
                }
                if (request != null)
                {
                    var options = new RaygunRequestMessageOptions();
                    requestMessage = RaygunRequestMessageBuilder.Build(request, options);
                }
            }
            return requestMessage;
        }
        private async Task StripAndSend(Exception exception, IList<string> tags, IDictionary userCustomData)
        {
            foreach (Exception e in this.StripWrapperExceptions(exception))
            {
                await this.Send(this.BuildMessage(e, tags, userCustomData));
            }
        }

        public string Url { get; set; } = "http://alpha.reactivedeveloper.com";
        public async Task Send(RaygunMessage raygunMessage)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(Url) })
            {
                try
                {
                    var message = SimpleJson.SerializeObject(raygunMessage);
                    var content = new StringContent(message);
                    await client.PostAsync("incidents", content);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error Logging Exception to ReactiveDeveloper.com{ex.Message}");
                }
            }


        }

        [ThreadStatic]
        private static RaygunRequestMessage m_currentRequestMessage;

        protected RaygunMessage BuildMessage(Exception exception, IList<string> tags, IDictionary userCustomData)
        {
            return RaygunMessageBuilder.New.SetHttpDetails(m_currentRequestMessage)
                .SetEnvironmentDetails()
                .SetMachineName(Environment.MachineName)
                .SetExceptionDetails(exception)
                .SetVersion(m_version)
                .SetTags(tags)
                .SetUserCustomData(userCustomData)
                .Build();
        }

        private readonly List<Type> m_wrapperExceptions = new List<Type>();
        public void AddWrapperExceptions(params Type[] wrapperExceptions)
        {
            foreach (var wrapper in wrapperExceptions)
            {
                if (!this.m_wrapperExceptions.Contains(wrapper))
                {
                    this.m_wrapperExceptions.Add(wrapper);
                }
            }
        }

        protected IEnumerable<Exception> StripWrapperExceptions(Exception exception)
        {
            if (exception != null && this.m_wrapperExceptions.Any(wrapperException => exception.GetType() == wrapperException && exception.InnerException != null))
            {
                var ex = exception as AggregateException;
                if (ex != null)
                {
                    foreach (var current in ex.InnerExceptions)
                    {
                        foreach (var current2 in this.StripWrapperExceptions(current))
                        {
                            yield return current2;
                        }
                    }
                }
                else
                {
                    foreach (var current3 in this.StripWrapperExceptions(exception.InnerException))
                    {
                        yield return current3;
                    }
                }
            }
            else
            {
                yield return exception;
            }
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