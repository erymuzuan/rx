using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpAdapterRouteTableProvider : IRouteTableProvider
    {
        public string GetEditorViewModel(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.http")
                return Properties.Resources.HttpAdapterJs;
            if (route.ModuleId == "viewmodels/adapter.http.operation")
                return Properties.Resources.HttpAdapterOperationJs;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.http")
                return Properties.Resources.HttpAdapterHtml;
            if (route.ModuleId == "viewmodels/adapter.http.operation")
                return Properties.Resources.HttpAdapterOperationHtml;
            return null;
        }

        public IEnumerable<JsRoute> Routes
        {
            get
            {
                return new[]{
                new JsRoute
                {
                    Caption = "Http Adapter",
                    GroupName = "Adapter",
                    IsAdminPage = true,
                    Route = "adapter.http/:id",
                    Title = "Http Adapter",
                    Icon = "fa fa-html5",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.http",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true,
                    StartPageRoute = "adapter.http/0"
                },
                new JsRoute
                {
                    Caption = "Web page operation",
                    GroupName = "Adapter",
                    IsAdminPage = false,
                    Route = "adapter.http.operation/:id/:uuid",
                    ModuleId = "viewmodels/adapter.http.operation",
                    Title = "Web page operation",
                    Icon = "fa fa-share-alt",
                    Nav = false,
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true
                }
                };
            }
        }
    }
}