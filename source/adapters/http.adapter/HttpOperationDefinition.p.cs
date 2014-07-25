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

        public string HttpMethod { get; set; }
        private readonly ObjectCollection<HttpHeaderDefinition> m_headerDefinitionCollection = new ObjectCollection<HttpHeaderDefinition>();

        public ObjectCollection<HttpHeaderDefinition> HeaderDefinitionCollection
        {
            get { return m_headerDefinitionCollection; }
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

        public string GetDefaultHeader(string key)
        {
            var hd = this.HeaderDefinitionCollection.SingleOrDefault(a => a.Name == key);
            return null == hd ? null : hd.DefaultValue;
        }

    }
}
