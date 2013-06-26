using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public static class Roles
    {
        public const string ADMIN_DASHBOARD = "admin_dashboard";
    }
    public partial class AppController
    {

        public async Task<ActionResult> ConfigJs()
        {
            var vm = new ApplicationConfigurationViewModel { StartModule = "admindashboard" };

            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);

            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var routes = await JsonConvert.DeserializeObjectAsync<JsRoute[]>(json, settings);
            vm.Routes.AddRange(routes.Where(r => User.IsInRole(r.role) || string.IsNullOrWhiteSpace(r.role)));

            return View(vm);
        }

        [ActionName("config.js")]
        public ActionResult Config(string id)
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            var script = this.RenderScript("ConfigJs");
            return Content(script);
        }


    }

    public class ApplicationConfigurationViewModel
    {
        private readonly ObjectCollection<JsRoute> m_routesCollection = new ObjectCollection<JsRoute>();

        public ObjectCollection<JsRoute> Routes
        {
            get { return m_routesCollection; }
        }

        public string StartModule { get; set; }
    }
}
