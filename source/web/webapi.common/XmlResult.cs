using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace Bespoke.Sph.WebApi
{
    public class XmlResult : IHttpActionResult
    {
        private readonly XmlDocument m_xml;
        private readonly CacheMetadata m_cache;
        private readonly HttpStatusCode m_statusCode;

        public XmlResult(XmlDocument xml, CacheMetadata cache = null,  HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_xml = xml;
            m_cache = cache;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_xml.OuterXml)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            m_cache?.SetMetadata(response);
            return Task.FromResult(response);
        }
    }
}