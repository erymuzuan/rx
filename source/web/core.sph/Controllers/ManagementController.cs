using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
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
                return Json(new { success = false, message = ex.Message, status = "No Broker" },JsonRequestBehavior.AllowGet);
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




    }
}