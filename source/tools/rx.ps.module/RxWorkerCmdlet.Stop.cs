using System;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Bespoke.Sph.Powershells;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
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

}
