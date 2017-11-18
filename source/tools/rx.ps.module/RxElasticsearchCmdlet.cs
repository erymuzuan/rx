using System.Management.Automation;

namespace Bespoke.Sph.RxPs
{

    [Cmdlet(VerbsLifecycle.Stop, "RxElasticsearch")]
    public class StopRxElasticsearchCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }

    [Cmdlet(VerbsCommon.Get, "RxElasticsearch")]
    public class GetRxElasticsearchCmdlet : RxCmdlet
    {
        protected override void ProcessRecord()
        {
            WriteObject("ToDO...");
        }

    }
}
