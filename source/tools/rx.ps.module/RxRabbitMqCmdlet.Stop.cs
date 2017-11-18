using System.Diagnostics;
using System.Management.Automation;
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
    
}
