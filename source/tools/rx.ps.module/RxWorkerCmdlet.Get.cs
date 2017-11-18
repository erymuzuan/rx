using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Bespoke.Sph.Powershells;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
 
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

    
}
