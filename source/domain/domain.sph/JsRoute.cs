

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Domain
{
    public  class JsRoute
    {
        public string Role { set; get; }
        public string GroupName { set; get; }
        public string Route { set; get; }
        public string ModuleId{ set; get; }
        public string Title{ set; get; }
        public bool Nav{ set; get; }
        public string Icon{ set; get; }
        public string Caption { set; get; }
        public JsRouteSetting Settings { set; get; }
        public bool ShowWhenLoggedIn { get; set; }
        public bool IsAdminPage { get; set; }
        public string StartPageRoute { get; set; }

        public static async Task<JsRoute[]> GetCustomRoutes()
        {
            var context = new SphDataContext();

            var entities = await context.GetListAsync<EntityDefinition, string>(x => x.Id != "0", x => x.Name);
            var forms = await context.GetListAsync<EntityForm, string>(x => x.Id != "0", x => x.Route);
            var views = await context.GetListAsync<EntityView, string>(x => x.Id != "0", x => x.Route);

            var list = new List<JsRoute>();
            list.AddRange(entities.Select(x => new JsRoute {Route = x}));
            list.AddRange(forms.Select(x => new JsRoute {Route = x}));
            list.AddRange(views.Select(x => new JsRoute {Route = x}));

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = $"{ConfigurationManager.WebPath}\\App_Data\\routes.config.json";
            if (System.IO.File.Exists(customRouteConfig))
            {
                var json = System.IO.File.ReadAllText(customRouteConfig);
                var customRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                list.AddRange(customRoutes);
            }
            return list.ToArray();
        }
    }
}