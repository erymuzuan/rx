using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsLifecycle.Stop, "RxRabbitMq")]
    public class StopRxRabbitMqCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteVerbose("[STOPPING] web.console.logger");
            WriteVerbose("RabbitMQ...[STOPPING]");

            var rabbitmqctl = $@"{ConfigurationManager.Home}\rabbitmq_server\sbin\rabbitmqctl.bat";
            var startInfo = new ProcessStartInfo
            {
                FileName = rabbitmqctl,
                Arguments = "stop",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            using (var stop = Process.Start(startInfo))
            {
                stop?.WaitForExit();
            }

            WriteVerbose("RabbitMQ... [STOPPED]");
        }
    }

    [Cmdlet(VerbsLifecycle.Start, "RxRabbitMq")]
    public class StartRxRabbitMqCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
            var rabbitMqBase = Environment.GetEnvironmentVariable("RABBITMQ_BASE", EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(rabbitMqBase) || !Directory.Exists(rabbitMqBase))
            {
                WriteWarning("Environment variable for RABBITMQ_BASE was not properly set");
                WriteWarning($"The value {rabbitMqBase} is not valid or does not exist");
                return;
            }
            WriteVerbose("Starting RabbitMq broker ...");
            WriteVerbose("RabbitMQ...[STARTING]");
            try
            {
                var rabbitMqServerBat = $@"{ConfigurationManager.Home}\rabbitmq_server\sbin\rabbitmq-server.bat";
                if (!File.Exists(rabbitMqServerBat))
                {
                    WriteWarning("CannotFind " + rabbitMqServerBat);
                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = rabbitMqServerBat,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Normal,

                };

                var rabbitMqServer = Process.Start(startInfo);

                if (null == rabbitMqServer) throw new InvalidOperationException("Cannot start RabbitMQ");
                rabbitMqServer.BeginOutputReadLine();
                rabbitMqServer.BeginErrorReadLine();
                rabbitMqServer.OutputDataReceived += (o, e) => WriteVerbose(e.Data);
                rabbitMqServer.ErrorDataReceived += (o, e) => WriteWarning(e.Data);



                var tcs = new TaskCompletionSource<bool>();
                void Started(object o, DataReceivedEventArgs e)
                {
                    if ($"{e.Data}".Contains("Starting broker"))
                        tcs.SetResult(true);
                    if ($"{e.Data}".Contains("ERROR"))
                    {
                        tcs.SetResult(false);
                    }
                    if ($"{e.Data}".Contains("erl_crash.dump"))
                    {
                        tcs.SetResult(false);
                    }
                }

                rabbitMqServer.OutputDataReceived += Started;
                rabbitMqServer.ErrorDataReceived += Started;
                tcs.Task.ContinueWith(_ =>
                {
                    if (_.Result)
                    {
                        WriteObject("RabbitMQ... [STARTED]");
                    }
                    else
                    {
                        WriteWarning("!Error starting RabbitMq, please check the log in the console");
                    }
                });
                tcs.Task.Wait(TimeSpan.FromMinutes(2));


            }
            catch (Exception ex)
            {
                WriteError(new ErrorRecord(ex, "", ErrorCategory.FromStdErr, this));
            }
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
            var rabbitmqctl = $@"{ConfigurationManager.Home}\rabbitmq_server\sbin\rabbitmqctl.bat";
            var info = new ProcessStartInfo(rabbitmqctl, "cluster_status -q")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            var process = Process.Start(info);
            if (null == process)
                throw new InvalidOperationException("Cannot start rabbitmqctl");
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(err))
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
