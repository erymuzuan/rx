using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.Models;
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
            var url = $"{ ConfigurationManager.ElasticSearchIndex}/request_log/_search";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, new StringContent(query));
                response.EnsureSuccessStatusCode();

                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                var responseString = await content.ReadAsStringAsync();
                return Content(responseString, "application/json");

            }

        }

    }
}