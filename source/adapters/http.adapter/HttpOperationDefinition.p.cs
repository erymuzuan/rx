using System.Linq;
using System.Net;
using Bespoke.Sph.Domain;

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
        public string RequestRouting { get; set; }
        public string ResponseMimeType { get; set; }
        public bool ResponseIsJsonArray { get; set; }
        public string HttpMethod { get; set; }
        public string HttpVersion { get; set; }
        private readonly ObjectCollection<HttpHeaderDefinition> m_requestHeaderDefinitionCollection = new ObjectCollection<HttpHeaderDefinition>();
        private readonly ObjectCollection<HttpHeaderDefinition> m_responseHeaderDefinitionCollection = new ObjectCollection<HttpHeaderDefinition>();

        public ObjectCollection<HttpHeaderDefinition> ResponseHeaderDefinitionCollection
        {
            get { return m_responseHeaderDefinitionCollection; }
        }

        public ObjectCollection<HttpHeaderDefinition> RequestHeaderDefinitionCollection
        {
            get { return m_requestHeaderDefinitionCollection; }
        }

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

        public string GetRequestHeader(string key)
        {
            var hd = this.RequestHeaderDefinitionCollection.SingleOrDefault(a => a.Name == key);
            return null == hd ? null : hd.DefaultValue;
        }

        public string GetResponsetHeader(string key)
        {
            var hd = this.ResponseHeaderDefinitionCollection.SingleOrDefault(a => a.Name == key);
            return null == hd ? null : hd.DefaultValue;
        }

    }
}
