using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Station.Windows;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ControlCenter
{
    public partial class App
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private readonly List<string> m_tags = new List<string>();
        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                WebConsoleServer.Default.Stop();
                WebConsoleServer.Default.StopConsume();
            }
            catch
            {
                //ignire
            }
            var entry = new LogEntry(e.ExceptionObject as Exception);
            var message = entry.ToString();

            var data = AddData(entry);
            try
            {
                this.StripAndSend(e.ExceptionObject as Exception, m_tags, data).Wait();
            }
            catch (Exception)
            {
                // ignored
            }
            this.MainWindow.Post(m =>
            {
                var window = new ErrorWindow(message);
                window.ShowDialog();
                this.Shutdown(-1);
            }, message);

        }
        private readonly List<Type> m_wrapperExceptions = new List<Type>();
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
        private async Task StripAndSend(Exception exception, IList<string> tags, IDictionary userCustomData)
        {
            foreach (Exception e in this.StripWrapperExceptions(exception))
            {
                await this.Send(this.BuildMessage(e, tags, userCustomData));
            }
        }
        public async Task Send(RaygunMessage raygunMessage)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(SphSettings.Load().IncidentUri) })
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



        protected RaygunMessage BuildMessage(Exception exception, IList<string> tags, IDictionary userCustomData)
        {
            var version = "unknown";
            string file = "..\\version.json";
            if (File.Exists(file))
            {
                var json = JObject.Parse(File.ReadAllText(file));
                var build = json.SelectToken("$.build").Value<int>();
                version = build.ToString();
            }
            return RaygunMessageBuilder.New
                .SetEnvironmentDetails()
                .SetMachineName(Environment.MachineName)
                .SetExceptionDetails(exception)
                .SetVersion(version)
                .SetTags(tags)
                .SetUserCustomData(userCustomData)
                .Build();
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


        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            WebConsoleServer.Default.Stop();
            WebConsoleServer.Default.StopConsume();

            var entry = new LogEntry(e.Exception);
            var message = entry.ToString();


            var data = AddData(entry);
            try
            {
                this.StripAndSend(e.Exception, m_tags, data).Wait();
            }
            catch (Exception)
            {
                // ignored
            }
            var window = new ErrorWindow(message);
            window.ShowDialog();
            e.Handled = true;
            this.Shutdown(-1);
        }
    }
}
