using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
   public partial class AppController
    {

        public async Task<ActionResult> ConfigJs()
        { 
            var username = User.Identity.Name;
            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == username);
            var vm = new ApplicationConfigurationViewModel { StartModule = profile.StartModule };
            
            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);
            
            var settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()};
            var routes = await JsonConvert.DeserializeObjectAsync<JsRoute[]>(json, settings);
            vm.Routes.AddRange(routes.Where(r => User.IsInRole(r.Role) || string.IsNullOrWhiteSpace(r.Role)));
            return View(vm);
        }

        [ActionName("config.js")]
        public ActionResult Config(string id)
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

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
