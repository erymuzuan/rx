using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

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
            
            var json2 ="[" + string.Join(",\r\n", ha.OperationDefinitionCollection.Select(x => x.ToJsonString())) + "]";
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

        public async Task<IHttpActionResult> Post([FromBody]HttpAdapter ora)
        {
            var validationErrors = (await ora.ValidateAsync()).ToArray();
            if (validationErrors.Any())
                return Json(validationErrors);

            await ora.OpenAsync();
            var result = await ora.CompileAsync();

            return Ok(new
            {
                success = true,
                status = "OK",
                result,
                message = "Your entity has been successfully published",
                id = ora.Name
            });
        }

        [Route("{id}")]
        public async Task<HttpResponseMessage> Patch(int id, [FromBody]HttpOperationDefinition operation)
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

            ha.OperationDefinitionCollection.Replace(op,operation);

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
    }
}
