using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
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
    public class ManagementController : Controller
    {
        [Route("{text}")]
        public async Task<ActionResult> Index(string text)
        {
            var provider = ObjectBuilder.GetObject<ISearchProvider>();
            var results = await provider.SearchAsync(text);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(results));
        }

        [Route("rabbitmq")]
        [NoCache]
        public async Task<ActionResult> InvokeRabbitMqManagementApi(string resource)
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
                    this.Response.ContentType = "application/json; charset=utf-8";
                    return Content(response);
                }
            }
            catch (NoSuchObjectDefinitionException ex)
            {
                return Json(new { success = false, message = ex.Message, status = "No Broker" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [Route("basic-get/{queue}")]
        public ActionResult LoadMessageFromQueue(string queue)
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
                return HttpNotFound("No more message in " + queue);

            if (string.IsNullOrWhiteSpace(result.BasicProperties.ReplyTo))
                result.BasicProperties.ReplyTo = "empty";


            var deathHeader = new XDeathHeader(result.BasicProperties.Headers);
            var routingKey = deathHeader.RoutingValuesKeys;
            var message = System.Text.Encoding.UTF8.GetString(result.Body);

            return Json(new { message, routingKey, deathHeader, note = "broken!!!! this will keep the connection until basicReject or ack is called" }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [Route("logs/{id:guid}")]
        public async Task<ActionResult> SearchById(string id)
        {
            var url = $"{ ConfigurationManager.ElasticSearchSystemIndex}/log/{id}";
            string responseString;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.GetAsync(url);
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return HttpNotFound();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                responseString = await content.ReadAsStringAsync();

            }
            var esJson = JObject.Parse(responseString);
            var source = esJson.SelectToken("$._source");

            return Content(source.ToString(), "application/json");

        }



        [HttpPost]
        [Route("request-logs/{from}/{to}")]
        public async Task<ActionResult> SearchRequestLogs(string from, string to, [RawRequestBody]string query)
        {
            var url = $"{ConfigurationManager.ElasticSearchIndex}_logs/request_log/_search";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticsearchLogHost);
                var response = await client.PostAsync(url, new StringContent(query));
                response.EnsureSuccessStatusCode();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                var responseString = await content.ReadAsStringAsync();
                return Content(responseString, "application/json");

            }

        }

        [HttpGet]
        [Route("endpoint-permissions")]
        public async Task<ActionResult> GetEndpointPermissionSettingsAsync()
        {
            string parent = this.Request.QueryString["parent"],
                controller = this.Request.QueryString["controller"],
                action = this.Request.QueryString["action"];

            var cache = ObjectBuilder.GetObject<ICacheManager>();
            var settings = cache.Get<List<EndpointPermissonSetting>>("endpoint-permissions");
            if (null == settings)
            {
                var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
                settings = (await repos.LoadAsync()).ToList();
                var source = $"{ConfigurationManager.SphSourceDirectory}\\EndpointPermissionSetting\\default.json";
                cache.Insert("endpoint-permissions", TimeSpan.FromSeconds(300), source);

            }

            var defaultItem = settings.Single(x => x.Parent == null);
            EndpointPermissonSetting item;

            if (!string.IsNullOrWhiteSpace(action))
            {
                item = settings.SingleOrDefault(x => x.Action == action && x.Controller == controller && x.Parent == parent);
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item, JsonRequestBehavior.AllowGet);
                }

                item = settings.SingleOrDefault(x => x.Controller == controller && x.Parent == parent && string.IsNullOrWhiteSpace(x.Action));
                if (null == item) item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action))
                        ?? defaultItem;
                item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrWhiteSpace(controller))
            {
                item = settings.SingleOrDefault(x => x.Controller == controller && x.Parent == parent && string.IsNullOrWhiteSpace(x.Action));
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item, JsonRequestBehavior.AllowGet);
                }

                item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action))
                       ?? defaultItem;
                item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            if (!string.IsNullOrWhiteSpace(parent))
            {
                item = settings.SingleOrDefault(x => x.Parent == parent && string.IsNullOrWhiteSpace(x.Controller) && string.IsNullOrWhiteSpace(x.Action));
                if (null != item)
                {
                    item.AddInheritedClaims(settings);
                    return Json(item, JsonRequestBehavior.AllowGet);
                }
                item = defaultItem;
                //   item.AddInheritedClaims(settings);
                item.MarkInherited();
                return Json(item, JsonRequestBehavior.AllowGet);
            }


            return Content(defaultItem.ToJson(), "application/json");
        }

        [HttpPost]
        [Route("endpoint-permissions")]
        public async Task<ActionResult> SaveEndpointPermissionSettingsAsync(EndpointPermissonSetting permisson)
        {
            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var settings = (await repos.LoadAsync()).ToList();
            var claims = permisson.Claims.ToList();
            claims.RemoveAll(x => x.Permission.StartsWith("i"));
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