using System.Collections.Generic;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class RestApiAdapterRouteProvider
    {

        public string GetEditorViewModel(JsRoute route)
        {
            switch (route.ModuleId)
            {
                case "viewmodels/adapter.restapi":
                    return "";
                case "viewmodels/adapter.restapi.endpoint":
                    return "";
            }
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            switch (route.ModuleId)
            {
                case "viewmodels/adapter.restapi":
                    var generators = ObjectBuilder.GetObject<IDeveloperService>().ActionCodeGenerators;
                    var code = new StringBuilder();
                    foreach (var ga in generators)
                    {
                        code.AppendLine($@"         
                                <!-- ko if : ko.unwrap($type) === ""{ga.GetType().GetShortAssemblyQualifiedName()}"" -->
                                <h3>{ga.Name}</h3>
                                {ga.GetDesignerHtmlView()}
                                <!-- /ko -->");
                    }
                    return "";
                case "viewmodels/adapter.restapi.sproc":
                    return "";
            }
            return null;
        }

        public IEnumerable<JsRoute> Routes
        {
            get
            {
                var list = new List<JsRoute> {new JsRoute
                {
                    Caption = "REST Api Adapter",
                    GroupName = "developers",
                    IsAdminPage = true,
                    Route = "adapter.restapi/:id",
                    Title = "REST Api Adapter",
                    Icon = "fa fa-windows",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.restapi",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true,
                    StartPageRoute = null
                },
                new JsRoute
                {
                    Caption = "REST Api Adapter",
                    GroupName = "developers",
                    IsAdminPage = true,
                    Route = "adapter.restapi.endpoint/:id/:endpoint",
                    Title = "REST Api Adapter Endpoint",
                    Icon = "fa fa-gg",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.restapi.endpoint",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true
                }};
                return list;
            }
        }
    }
}