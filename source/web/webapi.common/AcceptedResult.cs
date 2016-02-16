using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public class AcceptedResult : IHttpActionResult
    {
        public Uri LocationUri { get; }
        public object Result { get; }
        public HttpRequestMessage Request { get; }

        public AcceptedResult(string location, HttpRequestMessage request, object result)
        {
            LocationUri = new Uri(location);
            Result = result;
            Request = request;
        }
        public AcceptedResult(Uri locationUri, HttpRequestMessage request, object result)
        {
            LocationUri = locationUri;
            Result = result;
            Request = request;
        }
        public AcceptedResult(HttpRequestMessage request, object result)
        {
            Result = result;
            Request = request;
        }


        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            if (Request.Headers.Accept.Count == 0 || Request.Headers.Accept.Any(x => x.MediaType.Contains("json")))
                response.Content = new StringContent(JsonConvert.SerializeObject(Result));

            if (null != LocationUri)
                response.Headers.Location = LocationUri;

            return Task.FromResult(response);
        }
    }
}