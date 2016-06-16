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
        private readonly HttpStatusCode m_statusCode;

        public XmlResult(XmlDocument xml,  HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_xml = xml;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_xml.OuterXml)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            return Task.FromResult(response);
        }
    }
    public class BinaryResult : IHttpActionResult
    {
        private readonly byte[] m_content;
        private readonly string m_mimeType;
        private readonly HttpStatusCode m_statusCode;

        public BinaryResult(byte[] content, string mimeType,  HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            m_content = content;
            m_mimeType = mimeType;
            m_statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(m_statusCode)
            {
                Content = new ByteArrayContent(m_content)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(m_mimeType);
            return Task.FromResult(response);
        }
    }
}