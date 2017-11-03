using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxElasticsearch")]
    public class StopRxElasticsearchCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }

    [Cmdlet(VerbsLifecycle.Start, "RxElasticsearch")]
    public class StartRxElasticsearchCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
    [Cmdlet(VerbsCommon.Get, "RxElasticsearch")]
    public class GetRxElasticsearchCmdlet : PSCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
