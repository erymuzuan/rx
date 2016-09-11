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
        [HttpGet]
        [Route("hars/{id}/endpoints")]
        public async Task<IHttpActionResult> GetEndpoints(string id)
        {
            var builder = EndpointsBuilderFactory.Create(id);
            var endpoints = await builder.BuildAsync();

            var json = "[" + endpoints.JoinString(",", x => x.ToJsonString()) + "]";
            return Json(json);
        }
    }
}