using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class OracleAdapterRoute : IRouteTableProvider
    {
        public string GetEditorViewModel(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.oracle")
                return Properties.Resources.OracleAdapterJs;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.oracle")
                return Properties.Resources.OracleAdapterHtml;
            return null;
        }

        public IEnumerable<JsRoute> Routes
        {
            get
            {
                var list = new List<JsRoute> {new JsRoute
                {
                    Caption = "Oracle Adapter",
                    GroupName = "developers",
                    IsAdminPage = true,
                    Route = "adapter.oracle/:id",
                    Title = "Oracle Adapter",
                    Icon = "fa fa-database",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.oracle",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true
                }};
                return list;
            }
        }
    }
}