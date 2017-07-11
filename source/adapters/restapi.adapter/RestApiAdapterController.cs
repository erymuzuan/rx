using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("restapi-adapters")]
    public class RestApiAdapterController : BaseApiController
    {
        public RestApiAdapterController()
        {
            ServiceProvider.Init();
        }
        [HttpGet]
        [Route("hars/{id}/endpoints")]
        public async Task<IHttpActionResult> GetEndpoints(string id)
        {
            var factory = new EndpointsBuilderFactory();
            var sp = ObjectBuilder.GetObject<ServiceProvider>();
            var builders = await factory.CreateAsync(sp, id);
            var tasks = builders.Select(x => x.BuildAsync());
            var endpoints = await Task.WhenAll(tasks);

            var json = "[" + endpoints.JoinString(",", x => x.ToJsonString()) + "]";
            return Json(json);
        }

        [HttpPost]
        [Route("endpoints/{name}/build")]
        public async Task<IHttpActionResult> BuildEndpointAsync([JsonBody]RestApiOperationDefinition endpoint, string name)
        {
            await endpoint.BuildAsync(name);
            return Json(endpoint.ToJsonString());
        }


        [HttpPost]
        [Route("publish")]
        public async Task<IHttpActionResult> PublishAsync([JsonBody]RestApiAdapter adapter)
        {
            var noTables = adapter.TableDefinitionCollection.Count == 0;
            var noOps = 0 == adapter.OperationDefinitionCollection.Count;

            if (noTables && noOps)
            {
                return Ok(new { message = "No resources or endpoints specified", success = false, status = "Fail" });
            }
            await adapter.OpenAsync(true);
            var result = await adapter.CompileAsync();

            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Publish");
            }

            return Json(new
            {
                message = result.Result ? "Successfully compiled" : "There are errors in your adapter",
                errors = result.Errors.ToArray(),
                success = result.Result,
                status = "OK"
            });
        }

    }
}