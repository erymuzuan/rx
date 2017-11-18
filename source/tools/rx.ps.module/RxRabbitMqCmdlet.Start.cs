using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
using System.Threading.Tasks;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{

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
    
}
