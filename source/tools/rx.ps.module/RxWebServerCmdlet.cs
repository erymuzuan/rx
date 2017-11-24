using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxWebServer")]
    public class StopRxWebServerCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }


    [Cmdlet(VerbsCommon.Get, "RxWebServer")]
    public class GetRxWebServerCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
