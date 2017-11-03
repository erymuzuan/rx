using System.Management.Automation;

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
    public class GetRxRabbitMqCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
