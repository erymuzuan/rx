using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{
    public abstract class BaseApiController : ApiController
    {
        protected IDeveloperService DeveloperService => ObjectBuilder.GetObject<IDeveloperService>();
        static BaseApiController()
        {
            try
            {
                WebApi.DeveloperService.Init();
            }
            catch (Exception e)
            {// FOR UNIT TEST
                Console.WriteLine(e);
            }
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
            [JsonProperty("note")]
            public string Note { get; set; }
            [JsonProperty("doc")]
            public string Documentation { get; set; }
        }

        protected IHttpActionResult File(byte[] contents, string mimeType, string contentDisposition = null, int maxAge = 0, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new FileResult(contents, mimeType, contentDisposition, maxAge, statusCode);
            return response;
        }

        protected IHttpActionResult NotFound(string message)
        {
            return new NotFoundTextPlainActionResult(message, this.Request);
        }

        protected IHttpActionResult NotModified(CacheMetadata cache)
        {
            return new NotModifiedResult(this.Request, cache);
        }
        protected IHttpActionResult Invalid(HttpStatusCode statusCode, object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var response = new InvalidResult(statusCode, json);
            return response;
        }
        protected IHttpActionResult Invalid(object content)
        {
            var json = JsonConvert.SerializeObject(content);
            var response = new InvalidResult((HttpStatusCode)422, json);
            return response;
        }

        protected IHttpActionResult Invalid(BusinessRule[] brokenRules)
        {
            var response = new InvalidResult(this.Request, brokenRules);
            return response;
        }

        protected IHttpActionResult Html(string html)
        {
            var response = new HtmlResult(HttpStatusCode.OK, html);
            return response;
        }
        protected IHttpActionResult Javascript(string script)
        {
            var response = new JavascriptResult(HttpStatusCode.OK, script);
            return response;
        }

        protected IHttpActionResult Json(string json, CacheMetadata cache = null)
        {
            return new JsonCachedResult(json, cache);
        }
        protected IHttpActionResult Json<T>(T content, CacheMetadata cache, JsonSerializerSettings settings = null)
        {
            if (null == settings)
                settings = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(content, settings);
            return new JsonCachedResult(json, cache);
        }

        // TODO : we should return negotiable result, but for now no cache is added
        protected IHttpActionResult Ok<T>(T content, CacheMetadata cache)
        {
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            };
            var json = JsonConvert.SerializeObject(content, setting);
            return new JsonCachedResult(json, cache);
        }

        protected IHttpActionResult Ok<T>(T content, string mimeType, CacheMetadata cache = null)
        {
            if(typeof(T) == typeof(XmlDocument))
                return new XmlResult(content as XmlDocument, cache);
            return new BinaryResult(content as byte[], mimeType, cache);
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
        protected string GetMd5Hash(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", data.Select(x => x.ToString("x2")));

            }
        }
        protected override CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            };

            return base.Created(location, content);
        }

        protected virtual IHttpActionResult Accepted<T>(T data)
        {
            return new AcceptedResult(this.Request, data);
        }
        protected virtual IHttpActionResult Accepted<T>(string location, T data)
        {
            return new AcceptedResult(location, this.Request, data);
        }

        protected CreatedNegotiatedContentResult<T> Created<T>(Uri location, T content, JsonSerializerSettings settings)
        {
            this.Configuration.Formatters.JsonFormatter.SerializerSettings = settings;
            return base.Created(location, content);
        }
    }
}