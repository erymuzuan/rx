using System.Diagnostics;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{

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

}
