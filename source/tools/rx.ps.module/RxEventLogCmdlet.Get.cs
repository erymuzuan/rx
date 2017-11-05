using System;
using System.Management.Automation;
using System.Net.Http;
using Bespoke.Sph.RxPs.Domain;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsCommon.Get, "RxEventLog")]
    [OutputType(typeof(LogEntry))]
    public class GetRxEventLogCmdlet : RxCmdlet
    {
        private HttpClient m_client;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            m_client = new HttpClient {BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost)};
        }

        protected override void ProcessRecord()
        {

        }

        protected override void StopProcessing()
        {
            m_client.Dispose();
            base.StopProcessing();
        }

        protected override void EndProcessing()
        {
            m_client.Dispose();
            base.EndProcessing();
        }
    }
}
