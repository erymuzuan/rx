using System;
using System.Diagnostics;
using System.Management.Automation;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsCommon.Get, "RxRabbitMq")]
    [OutputType(typeof(RabbitMqServer))]
    public class GetRxRabbitMqCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
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
