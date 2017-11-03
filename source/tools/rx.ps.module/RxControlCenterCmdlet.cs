using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxControlCenter")]
    public class StopRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "RxControlCenter")]
    public class StartRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
    [Cmdlet(VerbsCommon.Get, "RxControlCenter")]
    public class GetRxControlCenterCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
