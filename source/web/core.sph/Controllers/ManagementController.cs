using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using Spring.Objects.Factory;

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

        [Route("rabbitmq")]
        [HttpGet]
        public async Task<IHttpActionResult> InvokeRabbitMqManagementApi([FromUri(Name = "resource")]string resource)
        {
            var url = "api/" + resource;
            try
            {
                using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(ConfigurationManager.RabbitMqUserName, ConfigurationManager.RabbitMqPassword) })
                using (var client = new HttpClient(handler))
                {
                    var uri = $"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}/";
                    client.BaseAddress = new Uri(uri);

                    var response = await client.GetStringAsync(url);
                    return Json(response);
                }
            }
            catch (NoSuchObjectDefinitionException ex)
            {
                return Json(new { success = false, message = ex.Message, status = "No Broker" });
            }
        }

        [HttpPost]
        [Route("basic-get/{queue}")]
        public IHttpActionResult LoadMessageFromQueue(string queue)
        {
            var factory = new ConnectionFactory
            {
                VirtualHost = ConfigurationManager.RabbitMqVirtualHost,
                HostName = ConfigurationManager.RabbitMqHost,
                UserName = ConfigurationManager.RabbitMqUserName,
                Password = ConfigurationManager.RabbitMqPassword,
                Port = ConfigurationManager.RabbitMqPort,
            };
            var conn = factory.CreateConnection();
            var model = conn.CreateModel();

            var result = model.BasicGet(queue, false);
            if (null == result)
                return NotFound("No more message in " + queue);

            if (string.IsNullOrWhiteSpace(result.BasicProperties.ReplyTo))
                result.BasicProperties.ReplyTo = "empty";


            var deathHeader = new XDeathHeader(result.BasicProperties.Headers);
            var routingKey = deathHeader.RoutingValuesKeys;
            var message = System.Text.Encoding.UTF8.GetString(result.Body);

            return Json(new { message, routingKey, deathHeader, note = "broken!!!! this will keep the connection until basicReject or ack is called" });

        }

        [HttpGet]
        [Route("logs/{id:guid}")]
        public async Task<IHttpActionResult> SearchById(string id)
        {
            var url = $"{ ConfigurationManager.ElasticSearchSystemIndex}/log/{id}";
            string responseString;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticsearchLogHost);
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return NotFound();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                responseString = await content.ReadAsStringAsync();

            }
            var esJson = JObject.Parse(responseString);
            var source = esJson.SelectToken("$._source");

            return Json(source.ToString());

        }



        [HttpPost]
        [PostRoute("request-logs/{from}/{to}")]
        public async Task<IHttpActionResult> SearchRequestLogs(string from, string to, [RawBody]string query)
        {
            var url = $"{ConfigurationManager.ElasticSearchIndex}_logs/request_log/_search";
            using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticsearchLogHost) })
            {
                var response = await client.PostAsync(url, new StringContent(query));
                response.EnsureSuccessStatusCode();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                var responseString = await content.ReadAsStringAsync();
                return Json(responseString);

            }

        }

        [HttpGet]
        [Route("workflow-endpoints")]
        public IHttpActionResult GetWorkflowEndpoints()
        {
            var context = new SphDataContext();
            var wds = context.LoadFromSources<WorkflowDefinition>();
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
        public IHttpActionResult GetAdapterEndpoints()
        {
            var context = new SphDataContext();
            var adapters = context.LoadFromSources<Adapter>();
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