using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;

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
        public async Task<ActionResult> InvokeRabbitMqManagementApi(string resource)
        {
            var url = "api/" + resource;
            dynamic broker = ObjectBuilder.GetObject("IBrokerConnection");
            using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(broker.UserName, broker.Password) })
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri(broker.ManagementScheme + "://" + broker.Host + ":" + broker.ManagementPort + "/");

                var response = await client.GetStringAsync(url);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(response);

            }
        }


        [Route("basic-get/{queue}")]
        public ActionResult LoadMessageFromQueue(string queue)
        {
            dynamic broker = ObjectBuilder.GetObject("IBrokerConnection");
            var factory = new ConnectionFactory
          {
              VirtualHost = broker.VirtualHost,
              HostName = broker.Host,
              UserName = broker.UserName,
              Password = broker.Password,
              Port = broker.Port,
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

            return Json(new {message, routingKey, deathHeader, note="broken!!!! this will keep the connection until basicReject or ack is called"}, JsonRequestBehavior.AllowGet);

        }




    }
}