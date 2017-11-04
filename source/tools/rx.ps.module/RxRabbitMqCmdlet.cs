using System;
using System.Diagnostics;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsLifecycle.Stop, "RxRabbitMq")]
    public class StopRxRabbitMqCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "RxRabbitMq")]
    public class StartRxRabbitMqCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }
    }

    [Cmdlet(VerbsCommon.Get, "RxRabbitMq")]
    [OutputType(typeof(RabbitMqServer))]
    public class GetRxRabbitMqCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
            /*
             F:\project\work\entt.snb.v2 [dev.local master ↓8 +0 ~2 -0 !]> rabbitmqctl cluster_status
Cluster status of node rabbit@WS30 ...
[{nodes,[{disc,[rabbit@WS30]}]},
 {running_nodes,[rabbit@WS30]},
 {cluster_name,<<"rabbit@WS30">>},
 {partitions,[]},
 {alarms,[{rabbit@WS30,[]}]}]
F:\project\work\entt.snb.v2 [dev.local master ↓8 +0 ~2 -0 !]> rabbitmqctl stop_app
Stopping node rabbit@WS30 ...
F:\project\work\entt.snb.v2 [dev.local master ↓8 +0 ~2 -0 !]> rabbitmqctl stop
Stopping and halting node rabbit@WS30 ...
F:\project\work\entt.snb.v2 [dev.local master ↓8 +0 ~2 -0 !]> rabbitmqctl cluster_status
Cluster status of node rabbit@WS30 ...
Error: unable to connect to node rabbit@WS30: nodedown

DIAGNOSTICS
===========

attempted to contact: [rabbit@WS30]

rabbit@WS30:
  * connected to epmd (port 4369) on WS30
  * epmd reports: node 'rabbit' not running at all
                  no other nodes on WS30
  * suggestion: start the node

current node details:
- node name: 'rabbitmq-cli-04@WS30'
- home dir: C:\Users\erymu
- cookie hash: a8m8pEHk6QJiQ2ioD1AYdA==

F:\project\work\entt.snb.v2 [dev.local master ↓8 +0 ~2 -0 !]>
             */
            var info = new ProcessStartInfo
            {
                FileName = "rabbitmqctl.bat",
                Arguments = "cluster_status",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            var process = Process.Start(info);
            if (null == process)
                throw new InvalidOperationException("Cannot start rabbitmqctl");
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            WriteError(new ErrorRecord(new Exception(err), "", ErrorCategory.FromStdErr, this));
            process.WaitForExit();

            var rabbit = new RabbitMqServer
            {
                Status = output.Contains("unable to connect to node") ? "Stopped" : "Running",
                RabbitMqBase = Environment.GetEnvironmentVariable("RABBITMQ_BASE")
            };
            WriteObject(rabbit);
        }

    }
}
