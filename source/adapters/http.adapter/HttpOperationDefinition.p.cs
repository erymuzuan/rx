using System.Collections.Generic;
using System.Net;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class HttpOperationDefinition
    {
        private string m_url;
        public bool IsLoginPage { get; set; }
        public bool IsLoginOperation { get; set; }
        public bool IsLoginRequired { get; set; }
        public bool EnsureSuccessStatusCode { get; set; }
        public HttpStatusCode ExpectedStatusCode { get; set; }
        public bool FollowRedirect { get; set; }
        public long? Timeout { get; set; }
        public string GetRequestRoute { get; set; }

        public string HttpMethod { get; set; }
        public Dictionary<string, string> RequestHeaders { get; private set; }

        public string Url
        {
            get { return m_url; }
            set
            {

                if (string.IsNullOrWhiteSpace(this.Name))
                {
                }
                m_url = value;
            }
        }

    }
}
