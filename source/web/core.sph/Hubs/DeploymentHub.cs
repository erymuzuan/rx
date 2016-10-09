using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;


namespace Bespoke.Sph.Web.Hubs
{
    [Authorize(Roles = "developers,administrators")]
    public class DeploymentHub : Hub
    {

        public IEnumerable<string> GetDeploymentEnvironments()
        {
            var configs = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}",
                    "*.json")
                .Select(f => f.DeserializeFromJsonFile<WorkersConfig>());

            return configs.Select(x => x.Environment).Distinct();
        }

        public IEnumerable<WorkersConfig> GetConfigs(string env)
        {
            var configs = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}",
                    "*.json")
                .Select(f => f.DeserializeFromJsonFile<WorkersConfig>());

            return configs.Where(x => x.Environment == env);
        }
        public IEnumerable<WebServerConfig> GetWebServerConfigs(string env)
        {
            var configs = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WebServerConfig)}",
                    "*.json")
                .Select(f => f.DeserializeFromJsonFile<WebServerConfig>());

            return configs.Where(x => x.Environment == env);
        }

        public void AddWorkersConfig(WorkersConfig config)
        {
            config.Id = $"{config.Environment}.{config.Name.ToIdFormat()}";
            config.CreatedBy = Context.User.Identity.Name;
            config.ChangedBy = Context.User.Identity.Name;
            config.CreatedDate = DateTime.Now;
            config.ChangedDate = DateTime.Now;
            config.WebId = config.Id;

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json",
                config.ToJson());
        }

        public void SaveWebServer(WebServerConfig config)
        {
            config.Id = $"{config.Environment}.{config.Name.ToIdFormat()}";
            config.ChangedBy = Context.User.Identity.Name;
            config.ChangedDate = DateTime.Now;
            var folder = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(WebServerConfig)}";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            File.WriteAllText($"{folder}\\{config.Id}.json",
                config.ToJson());
        }
        public void SaveWorkersConfig(WorkersConfig config)
        {
            config.Id = $"{config.Environment}.{config.Name.ToIdFormat()}";
            config.ChangedBy = Context.User.Identity.Name;
            config.ChangedDate = DateTime.Now;

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json",
                config.ToJson());
        }

        public void DeleteWorkersConfig(WorkersConfig config)
        {
            File.Delete($"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{config.Id}.json");
        }

        public List<string> GetComputers()
        {
            var computerNames = new List<string>();

            using (var entry = new DirectoryEntry("LDAP://org.1mdb"))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = ("(objectClass=computer)");

                    // No size limit, reads all objects
                    searcher.SizeLimit = 0;

                    // Read data in pages of 250 objects. Make sure this value is below the limit configured in your AD domain (if there is a limit)
                    searcher.PageSize = 250;

                    // Let searcher know which properties are going to be used, and only load those
                    searcher.PropertiesToLoad.Add("name");

                    foreach (System.DirectoryServices.SearchResult resEnt in searcher.FindAll())
                    {
                        // Note: Properties can contain multiple values.
                        if (resEnt.Properties["name"].Count > 0)
                        {
                            var computerName = (string)resEnt.Properties["name"][0];
                            computerNames.Add(computerName);
                        }
                    }
                }
            }

            return computerNames;
        }

        public async Task<object> ExecutePowershell(string script, IProgress<PowershellScriptProgress> progress)
        {
            var tcs = new TaskCompletionSource<object>();
            using (var ps = PowerShell.Create())
            {
                ps.AddScript(script);

                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    object message = "";
                    if (null != psobjects)
                    {
                        // TODO : convert to object or json
                        var xml = psobjects[e.Index];
                        message = xml;
                    }
                    var data = new PowershellScriptProgress { Event = "DataAdded", Result = message };
                    progress.Report(data);
                };
                ps.Streams.Error.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    var message = "";
                    if (null != psobjects)
                    {
                        var o = psobjects[e.Index];
                        message = o.ToString();
                    }
                    var data = new PowershellScriptProgress { Event = "Error", Result = message };
                    progress.Report(data);
                };
                ps.Streams.Verbose.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    var message = "";
                    if (null != psobjects)
                    {
                        var o = psobjects[e.Index];
                        message = o.ToString();
                    }
                    var data = new PowershellScriptProgress { Event = "Verbose", Result = message };
                    progress.Report(data);
                };
                ps.Streams.Warning.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    object message = "";
                    if (null != psobjects)
                    {
                        var o = psobjects[e.Index];
                        message = JsonConvert.SerializeObject(o);
                    }
                    var data = new PowershellScriptProgress { Event = "Warning", Result = message };
                    progress.Report(data);
                };
                ps.Streams.Debug.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    var message = "";
                    if (null != psobjects)
                    {
                        var o = psobjects[e.Index];
                        message = o.ToString();
                    }
                    var data = new PowershellScriptProgress { Event = "Debug", Result = message };
                    progress.Report(data);
                };
                ps.Streams.Progress.DataAdded += (s, e) =>
                {
                    var psobjects = s as PSDataCollection<PSObject>;
                    var message = "";
                    if (null != psobjects)
                    {
                        var o = psobjects[e.Index];
                        message = o.ToString();
                    }
                    var data = new PowershellScriptProgress { Event = "Progress", Result = message };
                    progress.Report(data);
                };
                ps.InvocationStateChanged += (s, e) =>
                {
                    if (e.InvocationStateInfo.State == PSInvocationState.Completed)
                        tcs.TrySetResult(e);
                };

                var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    progress.Report(new PowershellScriptProgress { Event = "Waiting for the pipe line" });
                    await Task.Delay(500);
                }

            }
            return tcs.Task;
        }

        public class PowershellScriptProgress
        {
            public string Event { get; set; }
            public DataAddedEventArgs DataAdded { get; set; }
            public object Result { get; set; }
        }
    }
}