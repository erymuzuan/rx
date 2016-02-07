using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public abstract class BaseApiController : ApiController
    {
        protected IDeveloperService DeveloperService => ObjectBuilder.GetObject<IDeveloperService>();
        static BaseApiController()
        {
            WebApi.DeveloperService.Init();
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

        public HttpResponseMessage File(byte[] contents, string mimeType)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(contents)
            };
            response.Content.Headers.Add("Content-Type", mimeType);

            return response;
        }

        public IHttpActionResult NotFound(string message)
        {
            return new NotFoundTextPlainActionResult(message, this.Request);
        }
        protected IHttpActionResult Invalid(object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var response = new InvalidResult((HttpStatusCode)422, json);
            return response;
        }

        protected override OkNegotiatedContentResult<T> Ok<T>(T content)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            };
            return base.Ok(content);
        }
        protected OkNegotiatedContentResult<T> Ok<T>(T content, JsonSerializerSettings settings)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;
            return base.Ok(content);
        }

        protected override CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            };

            return base.Created(location, content);
        }
        protected CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content, JsonSerializerSettings settings)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;
            return base.Created(location, content);
        }
    }
}