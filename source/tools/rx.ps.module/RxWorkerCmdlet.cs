using System.Diagnostics;
using System.Management.Automation;
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
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }
        
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "RxWorker")]
    [OutputType(typeof(Worker))]
    public class StartRxWorkerCmdlet : RxCmdlet
    {
        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT, Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true)]
        public string Name { get; set; }

        [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Environment { get; set; }

        [Parameter(ParameterSetName = RxWorkerCmdlet.PARAMETER_SET_DEFAULT, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Configuration { get; set; }

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

            var info = new ProcessStartInfo
            {
                FileName = $"{ConfigurationManager.Home}\\subscribers.host\\workers.console.runner.exe",
                Arguments = $"{debug}{verbose}/log:console "
                            + $"/config:{this.Configuration} "
                            + $" /v:{RxApplicationName} /u:{ConfigurationManager.RabbitMqUserName} /p:{ConfigurationManager.RabbitMqPassword} /h:{ConfigurationManager.RabbitMqHost} "
                            + $"/out:{this.LogDirectory} /outSize:100KB /outSwitch:{TraceSwitch}",
                UseShellExecute = UseShellExecute
            };
           // info.Verbs

            /*
                Environment =
                {
                    {"RxWorkerName",this.Name},
                    {"RxWorkerEnvironment",this.Environment},
                    {"RxWorkerConfiguration",this.Configuration},
                },*/
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
        [Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
