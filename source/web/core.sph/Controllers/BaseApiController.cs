using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class InvalidResult : IHttpActionResult
    {
        private readonly string m_message;
        private readonly HttpStatusCode m_statusCode;

        public InvalidResult(HttpStatusCode statusCode, string message)
        {
            m_statusCode = statusCode;
            m_message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(m_statusCode)
            {
                Content = new StringContent(m_message)
            };
            return Task.FromResult(response);
        }
    }
    public abstract class BaseApiController : ApiController
    {
        protected IDeveloperService DeveloperService => ObjectBuilder.GetObject<IDeveloperService>();
        static BaseApiController()
        {
            Dependencies.DeveloperService.Init();
        }
        [JsonObject("_link")]
        public class Link
        {
            public Link()
            {

            }
            public Link(string rel, string href, string method)
            {
                this.Method = method;
                this.Href = href;
                this.Rel = rel;
            }
            [JsonProperty("method")]
            public string Method { get; set; }
            [JsonProperty("href")]
            public string Href { get; set; }
            [JsonProperty("rel")]
            public string Rel { get; set; }
        }


        protected IHttpActionResult Invalid(object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var response = new InvalidResult((HttpStatusCode)422, json);
            return response;
        }

        protected OkNegotiatedContentResult<T> Ok<T>(T content, bool auto)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            return base.Ok(content);
        }

        protected CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content, bool auto)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };

            return base.Created(location, content);
        }
    }
}