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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [RoutePrefix("httpadapter")]
    public class HttpAdapterController : ApiController
    {
        [Route("operations/{harStoreId}")]
        public async Task<HttpResponseMessage> GetOperations(string harStoreId)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var json = await store.GetContentAsync(harStoreId);
            var temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, json.Content);

            var ha = new HttpAdapter { Har = temp };
            await ha.OpenAsync();

            var json2 = "[" + string.Join(",\r\n", ha.OperationDefinitionCollection.Select(x => x.ToJsonString())) + "]";
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(json2, Encoding.UTF8)
            };
            return response;
        }

        [Route("operation/{id:int}/{uuid}")]
        public async Task<HttpResponseMessage> GetOperation(int id, string uuid)
        {
            var context = new SphDataContext();
            var adapters = context.CreateQueryable<Adapter>();
            var query = adapters.Where(x => x.AdapterId == id);
            var ha = (await context.LoadAsync(query)).ItemCollection.SingleOrDefault();
            if (null == ha)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var op = ha.OperationDefinitionCollection.SingleOrDefault(o => o.Uuid == uuid);
            if (null == op)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new JsonContent(op.ToJsonString(), Encoding.UTF8)
            };

            return response;
        }

        [Route("publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]HttpAdapter adapter)
        {
            var validationErrors = (await adapter.ValidateAsync()).ToArray();
            if (validationErrors.Any())
                return Json(validationErrors);
            adapter.Tables = new AdapterTable[] { };
            var result = await adapter.CompileAsync();

            var context = new SphDataContext();
            var adapters = context.CreateQueryable<Adapter>();
            var query = adapters.Where(x => x.AdapterId == adapter.AdapterId);
            var ha = (await context.LoadAsync(query)).ItemCollection.SingleOrDefault();
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

        [Route("{id:int}")]
        public async Task<HttpResponseMessage> Patch(int id, [JsonBody]HttpOperationDefinition operation)
        {
            var context = new SphDataContext();
            var adapters = context.CreateQueryable<Adapter>();
            var query = adapters.Where(x => x.AdapterId == id);
            var ha = (await context.LoadAsync(query)).ItemCollection.SingleOrDefault();
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
        [Route("text/{id:int}/{method}")]
        public async Task<IHttpActionResult> Text(int id, string method, [FromUri]string url)
        {
            var context = new SphDataContext();
            var adapters = context.CreateQueryable<Adapter>();
            var query = adapters.Where(x => x.AdapterId == id);
            var ha = (await context.LoadAsync(query)).ItemCollection.SingleOrDefault() as HttpAdapter;
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
                var operations = from j in entries
                                 select new HttpOperationDefinition(j)
                                 {
                                     Url = j.SelectToken("request.url").Value<string>(),
                                     HttpMethod = j.SelectToken("request.method").Value<string>(),
                                     Uuid = Guid.NewGuid().ToString(),
                                     WebId = j.SelectToken("response.content.text").Value<string>()
                                 };
                var d = operations.SingleOrDefault(o => o.HttpMethod == method && o.Url == url);
                if (null != d)
                    return Content( HttpStatusCode.OK, d.WebId);

            }

            return NotFound();

        }
    }
}
