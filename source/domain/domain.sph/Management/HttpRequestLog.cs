using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class HttpRequestLog : DomainObject
    {
        public long Elapsed { get; set; }
        public string Time { get; set; }
        public string User { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public HttpRequestPayload Request { get; set; }
        public HttpResponsePayload Response { get; set; }
    }

    public class HttpRequestPayload
    {
        public string Path { get; set; }
        public string PathBase { get; set; }
        public string Host { get; set; }
        public string[] Accepts { get; set; }
        public string RemoteIpAddress { get; set; }
        public string CacheControl { get; set; }
        public bool IsSecure { get; set; }
        public string LocalIpAddress { get; set; }
        public string Method { get; set; }
        public string Protocol { get; set; }
        public string Scheme { get; set; }
    }
    public class HttpResponsePayload
    {
        public long? ContentLength { get; set; }
        public string ContentType { get; set; }
        public string ReasonPhrase { get; set; }
        public int StatusCode { get; set; }
    }

    public interface IMeteringRepository
    {
        // fire and forget
        void Log(HttpRequestLog request);
        Task<LoadOperation<HttpRequestLog>> SearchAsync(QueryDsl query);
    }

}
