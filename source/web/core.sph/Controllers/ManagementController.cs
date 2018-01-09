using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "administrators")]
    [RoutePrefix("management-api")]
    public class ManagementController : BaseApiController
    {
        [Route("{text}")]
        public async Task<IHttpActionResult> Index(string text)
        {
            var provider = ObjectBuilder.GetObject<ISearchProvider>();
            var results = await provider.SearchAsync(text);
            return Json(results);
        }

        [Route("message-broker")]
        [HttpGet]
        public async Task<IHttpActionResult> InvokeRabbitMqManagementApi([FromUri(Name = "resource")]string resource)
        {
            var broker = ObjectBuilder.GetObject<IMessageBroker>();
            var stat = await broker.GetStatisticsAsync(resource);
            return Json(stat);

        }

        [HttpPost]
        [Route("basic-get/{queue}")]
        public async Task<IHttpActionResult> LoadMessageFromQueue(string queue)
        {
            var broker = ObjectBuilder.GetObject<IMessageBroker>();

            var result = await broker.GetMessageAsync(queue);
            if (null == result)
                return NotFound("No more message in " + queue);

            if (string.IsNullOrWhiteSpace(result.ReplyTo))
                result.ReplyTo = "empty";


            var deathHeader = new XDeathHeader(result.Headers);
            var routingKey = deathHeader.RoutingValuesKeys;
            var message = System.Text.Encoding.UTF8.GetString(result.Body);

            return Json(new { message, routingKey, deathHeader, note = "broken!!!! this will keep the connection until basicReject or ack is called" });

        }

        [HttpGet]
        [Route("logs/{id:guid}")]
        public async Task<IHttpActionResult> SearchById(string id)
        {
            var repos = ObjectBuilder.GetObject<ILoggerRepository>();
            var source = await repos.LoadOneAsync(id);

            return Json(source);

        }



        [HttpPost]
        [PostRoute("request-logs/{from}/{to}")]
        public async Task<IHttpActionResult> SearchRequestLogs(string from, string to, [RawBody]string text, [ContentType]MediaTypeHeaderValue contentType)
        {
            var parser = QueryParserFactory.Instance.Get(null, contentType?.MediaType ?? "appplication/json+elasticsearch");
            var query = parser.Parse(text, String.Empty);

            var repos = ObjectBuilder.GetObject<IMeteringRepository>();
            var lo = await repos.SearchAsync(query);
            return Json(lo);

        }

        [HttpGet]
        [Route("workflow-endpoints")]
        public async Task<IHttpActionResult> GetWorkflowEndpoints()
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var wds = await repos.LoadAsync<WorkflowDefinition>();
            var list = from w in wds
                       let receives = w.ActivityCollection.OfType<ReceiveActivity>().ToArray()
                       where receives.Length > 0
                       select new
                       {
                           Name = w.WorkflowTypeName,
                           Children = receives.Select(x => new
                           {
                               x.Name,
                               Action = x.MethodName ?? "!!null!!"
                           }).ToArray()
                       };
            var json = JsonConvert.SerializeObject(list);
            return Json(json);
        }

        [HttpGet]
        [Route("adapter-endpoints")]
        public async Task<IHttpActionResult> GetAdapterEndpoints()
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var adapters = await repos.LoadAsync<Adapter>();
            var list = from w in adapters
                       let tables = w.TableDefinitionCollection.Select(x => new
                       {
                           x.Name,
                           Actions = x.ControllerActionCollection.Where(c => c.IsEnabled).Select(c => c.GetActionNames(x, w)).SelectMany(c => c).ToArray()
                       })
                       let ops = w.OperationDefinitionCollection.Select(x => x.MethodName)
                       select new
                       {
                           w.Name,
                           Operations = ops.ToArray(),
                           Tables = tables.ToArray()
                       };
            var json = JsonConvert.SerializeObject(list);
            return Json(json);
        }

        [HttpGet]
        [Route("endpoint-permissions")]
        public async Task<IHttpActionResult> GetEndpointPermissionSettingsAsync([FromUri(Name = "parent")]string parent = null,
            [FromUri(Name = "controller")]string controller = null,
            [FromUri(Name = "action")]string action = null)
        {

            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var settings = (await repos.LoadAsync()).ToArray();

            if (string.IsNullOrWhiteSpace(parent) && string.IsNullOrWhiteSpace(controller) && string.IsNullOrWhiteSpace(action))
            {
                return Json(settings);
            }


            var defaultItem = settings.Single(x => x.Parent == null);
            EndpointPermissonSetting item;

            if (!string.IsNullOrWhiteSpace(action))
            {
                item = settings.SingleOrDefault(x => x.Action == action && x.Controller == controller && x.Parent == parent);
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item);
                }

                item = settings.SingleOrDefault(x => x.Controller == controller && x.Parent == parent && string.IsNullOrWhiteSpace(x.Action));
                if (null == item) item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action))
                        ?? defaultItem;
                item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item);
            }
            if (!string.IsNullOrWhiteSpace(controller))
            {
                item = settings.SingleOrDefault(x => x.Controller == controller && x.Parent == parent && string.IsNullOrWhiteSpace(x.Action));
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item);
                }

                item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action))
                       ?? defaultItem;
                item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item);
            }
            if (!string.IsNullOrWhiteSpace(parent))
            {
                item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action));
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item);
                }
                item = defaultItem;
                //   item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item);
            }


            return Json(defaultItem.ToJson());
        }

        [HttpPost]
        [Route("endpoint-permissions")]
        public async Task<IHttpActionResult> SaveEndpointPermissionSettingsAsync([RawBody]string json)
        {
            var permisson = EndpointPermissonSetting.Parse(json);

            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var settings = (await repos.LoadAsync()).ToList();
            var claims = permisson.Claims.ToList();
            claims.RemoveAll(x => x?.Permission?.StartsWith("i") ?? false);
            permisson.Claims = claims.ToArray();

            settings.RemoveAll(
                x =>
                    x.Parent == permisson.Parent && x.Controller == permisson.Controller && x.Action == permisson.Action);
            settings.Add(permisson);
            await repos.SaveAsync(settings);
            return Json(new { success = true, status = "OK" });
        }

    }
}