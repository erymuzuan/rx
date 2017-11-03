using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Language;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Bespoke.Sph.Powershells;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    public class RxWorkerCmdlet
    {
        public const string PARAMETER_SET_DEFAULT = "default";
    }

    [Cmdlet(VerbsLifecycle.Stop, "RxWorker")]
    public class StopRxWorkerCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = "Names")]
        [ArgumentCompleter(typeof(WorkerProcessNameCompleter))]
        public string[] Names { get; set; }

        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Pid", Mandatory = true)]
        public int Pid { get; set; }

        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = nameof(Worker), Mandatory = true)]
        public Worker Worker { get; set; }


        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);
        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "Names")
            {
                var wps = from p in Process.GetProcessesByName("workers.console.runner")
                          let envs = p.ReadEnvironmentVariables()
                          let name = envs.ContainsKey("RxWorkerName") ? envs["RxWorkerName"] : "NA"
                          where this.Names.Contains(name, StringComparer.InvariantCultureIgnoreCase)
                          select p;
                foreach (var worker in wps)
                {
                    var h = worker.MainWindowHandle;
                    SetForegroundWindow(h);
                    SendKeys.SendWait("^c");

                }
            }
            if (this.ParameterSetName == "Pid")
            {
                var worker = Process.GetProcesses().SingleOrDefault(x => x.Id == Pid);
                WriteVerbose($"Worker {(worker == null ? "not " : "")}found");
                if (null == worker) return;
                IntPtr h = worker.MainWindowHandle;
                SetForegroundWindow(h);
                SendKeys.SendWait("^c");

            }
            if (this.ParameterSetName == nameof(Worker))
            {
                var worker = Process.GetProcesses().SingleOrDefault(x => x.Id == this.Worker.Pid);
                WriteVerbose($"Worker {(worker == null ? "not " : "")}found");
                if (null == worker) return;
                var h = worker.MainWindowHandle;
                SetForegroundWindow(h);
                SendKeys.SendWait("^c");

            }
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "RxWorker")]
    [OutputType(typeof(Worker))]
    public class StartRxWorkerCmdlet : RxCmdlet
    {
        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ArgumentCompleter(typeof(WorkerEnvironmentCompleter))]
        public string Environment { get; set; } = "dev";

        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        [ArgumentCompleter(typeof(WorkerConfigCompleter))]
        public string Configuration { get; set; } = "all";

        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT)]
        public string LogDirectory { get; set; } = ".\\logs\\workers.console.log";


        [Parameter(HelpMessage = "Trace switch for ConsoleLogger", ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT)]
        [ValidateSet("Debug", "Verbose", "Info", "Warning", "Error")]
        public string TraceSwitch { get; set; } = "Debug";

        [Parameter(HelpMessage = "Start the process in new window", ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT)]
        public SwitchParameter UseShellExecute { get; set; } = false;

        protected override void ProcessRecord()
        {
            var debug = MyInvocation.BoundParameters.ContainsKey("Debug") ? " /debug " : "";
            var verbose = MyInvocation.BoundParameters.ContainsKey("Verbose") ? " /verbose " : "";

            // hopefully this will attach it to the newly created worker process 
            System.Environment.SetEnvironmentVariable("RxWorkerName", this.Name);
            System.Environment.SetEnvironmentVariable("RxWorkerEnvironment", this.Environment);
            System.Environment.SetEnvironmentVariable("RxWorkerConfiguration", this.Configuration);

            var info = new ProcessStartInfo
            {
                FileName = $"{ConfigurationManager.Home}\\subscribers.host\\workers.console.runner.exe",
                Arguments = $"{debug}{verbose}/log:console "
                            + $"/config:{this.Configuration} /env:{this.Environment}"
                            + $" /v:{RxApplicationName} /u:{ConfigurationManager.RabbitMqUserName} /p:{ConfigurationManager.RabbitMqPassword} /h:{ConfigurationManager.RabbitMqHost} "
                            + $"/out:{this.LogDirectory} /outSize:100KB /outSwitch:{TraceSwitch}",
                UseShellExecute = UseShellExecute
            };
            WriteVerbose($"Starting worker {info.Arguments}");
            var worker = Process.Start(info);
            WriteObject(new Worker
            {
                Name = this.Name,
                Configuration = this.Configuration,
                Environment = this.Environment,
                Pid = worker?.Id,
                StartTime = worker?.StartTime
            });
        }

    }


    [Cmdlet(VerbsCommon.Get, "RxWorker")]
    [OutputType(typeof(Worker))]
    public class GetRxWorkerCmdlet : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipeline = true, ParameterSetName = "Names")]
        [ArgumentCompleter(typeof(WorkerProcessNameCompleter))]
        public string[] Names { get; set; }

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == "Names")
            {
                var wps = from p in Process.GetProcessesByName("workers.console.runner")
                          let envs = p.ReadEnvironmentVariables()
                          select new Worker
                          {
                              Name = envs.ContainsKey("RxWorkerName") ? envs["RxWorkerName"] : "NA",
                              Environment = envs.ContainsKey("RxWorkerEnvironment") ? envs["RxWorkerEnvironment"] : "NA",
                              Configuration = envs.ContainsKey("RxWorkerConfiguration") ? envs["RxWorkerConfiguration"] : "NA",
                              Pid = p.Id,
                              StartTime = p.StartTime,

                          };

                WriteObject(wps.Where(x => Names.Contains(x.Name)), true);
            }
            else
            {
                var wps = from p in Process.GetProcessesByName("workers.console.runner")
                          let envs = p.ReadEnvironmentVariables()
                          select new Worker
                          {
                              Name = envs.ContainsKey("RxWorkerName") ? envs["RxWorkerName"] : "NA",
                              Environment = envs.ContainsKey("RxWorkerEnvironment") ? envs["RxWorkerEnvironment"] : "NA",
                              Configuration = envs.ContainsKey("RxWorkerConfiguration") ? envs["RxWorkerConfiguration"] : "NA",
                              Pid = p.Id,
                              StartTime = p.StartTime,

                          };

                WriteObject(wps, true);

            }
        }

    }


    public class WorkerProcessNameCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var wps = from p in Process.GetProcessesByName("workers.console.runner")
                      let envs = p.ReadEnvironmentVariables()
                      select envs.ContainsKey("RxWorkerName") ? envs["RxWorkerName"] : "NA";
            return wps.ToArray();
        }
    }

    public class WorkerConfigCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\WorkersConfig", "*.json");
            var list = files.Select(Path.GetFileNameWithoutExtension)
                .Select(x => x.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            Console.WriteLine(string.Join(",", list));
            return list.ToArray();
        }
    }

    public class WorkerEnvironmentCompleter : IArgumentCompleter
    {
        IEnumerable<CompletionResult> IArgumentCompleter.CompleteArgument(string commandName,
            string parameterName,
            string wordToComplete,
            CommandAst commandAst,
            IDictionary fakeBoundParameters)
        {
            return GetAllowedNames().
                Where(new WildcardPattern(wordToComplete + "*", WildcardOptions.IgnoreCase).IsMatch).
                Select(s => new CompletionResult(s));
        }
        private static string[] GetAllowedNames()
        {
            var files = Directory.GetFiles($@"{ConfigurationManager.SphSourceDirectory}\WorkersConfig", "*.json");
            var list = files.Select(Path.GetFileNameWithoutExtension)
                    .Select(x => x.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
            Console.WriteLine("Environments..." + string.Join(",", list));
            return list.ToArray();
        }
    }
}
