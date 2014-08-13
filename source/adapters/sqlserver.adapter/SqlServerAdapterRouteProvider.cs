using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlServerAdapterRouteProvider : IRouteTableProvider
    {
        public string GetEditorViewModel(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.sqlserver")
                return Properties.Resources.SqlServerAdapterJs;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.sqlserver")
                return Properties.Resources.SqlServerAdapterHtml;
            return null;
        }

        public IEnumerable<JsRoute> Routes
        {
            get
            {
                var list = new List<JsRoute> {new JsRoute
                {
                    Caption = "MSSQL Server Adapter",
                    GroupName = "developers",
                    IsAdminPage = true,
                    Route = "adapter.sqlserver/:id",
                    Title = "MSSQL Server Adapter",
                    Icon = "fa fa-windows",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.sqlserver",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true
                }};
                return list;
            }
        }
    }
}