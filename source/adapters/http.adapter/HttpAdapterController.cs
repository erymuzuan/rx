using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("httpadapter")]
    public class HttpAdapterController : BaseApiController
    {
        [Route("operations/{harStoreId}")]
        public async Task<HttpResponseMessage> GetOperations(string harStoreId)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var json = await store.GetContentAsync(harStoreId);
            var temp = Path.GetTempFileName();
            System.IO.File.WriteAllBytes(temp, json.Content);

            var ha = new HttpAdapter { Har = temp };
            await ha.OpenAsync();

            var json2 = "[" + string.Join(",\r\n", ha.OperationDefinitionCollection.Select(x => x.ToJsonString())) + "]";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(json2, Encoding.UTF8)
            };
            return response;
        }

        [Route("operation/{id}/{uuid}")]
        public IHttpActionResult GetOperation(string id, string uuid)
        {
            var context = new SphDataContext();
            var ha = context.LoadOneFromSources<Adapter>(x =>x.Id == id);
            if (null == ha) return NotFound($"Cannot find HTTP adapter with id :{id}");

            var op = ha.OperationDefinitionCollection.SingleOrDefault(o => o.Uuid == uuid);
            if (null == op)
                return NotFound($"No operation with uuid :{uuid} is registered in {id}");
            
            return Json(op.ToJsonString());
        }

        [HttpPost]
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish(string id,[JsonBody]HttpAdapter adapter)
        {
            var validationErrors = (await adapter.ValidateAsync()).ToArray();
            if (validationErrors.Any())
                return Json(validationErrors);
            adapter.Tables = new AdapterTable[] { };
            var result = await adapter.CompileAsync();

            var context = new SphDataContext();
            var ha = context.LoadOneFromSources<Adapter>(x => x.Id == id);
            if (null == ha)
                return NotFound();

            using (var session = context.OpenSession())
            {
                session.Attach(ha);
                await session.SubmitChanges();
            }
            return Ok(new
            {
                success = true,
                status = "OK",
                result,
                message = "Your adapter has been successfully published",
                id = adapter.Name
            });
        }

        public async Task<IHttpActionResult> Post([FromBody]HttpAdapter adapter)
        {
            var validationErrors = (await adapter.ValidateAsync()).ToArray();
            if (validationErrors.Any())
                return Json(validationErrors);

            await adapter.OpenAsync();
            var result = await adapter.CompileAsync();

            return Ok(new
            {
                success = true,
                status = "OK",
                result,
                message = "Your entity has been successfully published",
                id = adapter.Name
            });
        }

        [Route("{id}/operation")]
        [HttpPatch]
        public async Task<HttpResponseMessage> UpdateOperation(string id, [JsonBody]HttpOperationDefinition operation)
        {
            var context = new SphDataContext();
            var ha = context.LoadOneFromSources<Adapter>(x => x.Id == id);
            if (null == ha)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var op = ha.OperationDefinitionCollection.SingleOrDefault(o => o.Uuid == operation.Uuid);
            if (null == op)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            ha.OperationDefinitionCollection.Replace(op, operation);

            using (var session = context.OpenSession())
            {
                session.Attach(ha);
                await session.SubmitChanges();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent((new
                {
                    success = true,
                    status = "OK",
                    message = "Your entity has been successfully published"
                }).ToJsonString(), Encoding.UTF8)
            };
            return response;
        }


        [HttpGet]
        [Route("text/{id}/{method}")]
        public async Task<IHttpActionResult> Text(string id, string method, [FromUri]string url)
        {
            var context = new SphDataContext();
            var ha = context.LoadOneFromSources<Adapter>(x => x.Id == id) as HttpAdapter;
            if (null == ha)
                return NotFound();

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var blob = await store.GetContentAsync(ha.Har);
            if (null == blob)
                return NotFound();

            using (var stream = new MemoryStream(blob.Content))
            using (var sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {

                var jo = JToken.ReadFrom(reader);
                var entries = jo.SelectTokens("$.log.entries").SelectMany(x => x);
                var operations = (from j in entries
                                  // where j.SelectToken("response.content.text") != null
                                  let textToken = j.SelectToken("response.content.text")
                                  let text = textToken == null ? string.Empty : textToken.Value<string>()
                                  let url302 = j.SelectToken("response.redirectURL")
                                  let url300 = url302 == null ? string.Empty : url302.Value<string>()
                                  select new HttpOperationDefinition(j)
                                  {
                                      Url = j.SelectToken("request.url").Value<string>(),
                                      HttpMethod = j.SelectToken("request.method").Value<string>(),
                                      Uuid = Guid.NewGuid().ToString(),
                                      WebId = text,// text
                                      CodeNamespace = url300// 302
                                  }).ToList();
                var d = operations.LastOrDefault(o => o.HttpMethod == method && o.Url.ToEmptyString().ToLowerInvariant().Contains(url.ToLowerInvariant()));
                if (null != d && !string.IsNullOrWhiteSpace(d.CodeNamespace))
                {
                    var redirectUrl = d.CodeNamespace;
                    Console.WriteLine("***************");
                    Console.WriteLine(redirectUrl);
                    Console.WriteLine("***************");
                    d = operations.LastOrDefault(o => o.HttpMethod == "GET" && o.Url == redirectUrl);
                }

                if (null != d)
                    return Content(HttpStatusCode.OK, d.WebId);

            }

            return NotFound();

        }

        [HttpPost]
        [Route("test")]
        public HttpResponseMessage TestRegex([FromBody]RegexTestViewModel regexTest)
        {
            var matches = Strings.RegexValues(regexTest.Html, regexTest.Pattern, regexTest.Group);
            var json = JsonConvert.SerializeObject(matches);
            var message = new HttpResponseMessage { Content = new JsonContent(json) };
            return message;
        }
    }
}
