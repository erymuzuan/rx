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
            if (route.ModuleId == "viewmodels/adapter.sqlserver.sproc")
                return Properties.Resources.SprocJs;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.sqlserver")
                return Properties.Resources.SqlServerAdapterHtml;
            if (route.ModuleId == "viewmodels/adapter.sqlserver.sproc")
                return Properties.Resources.SprocHtml;
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
                },
                new JsRoute
                {
                    Caption = "MSSQL Server Adapter",
                    GroupName = "developers",
                    IsAdminPage = true,
                    Route = "adapter.sqlserver.sproc/:id/:sproc",
                    Title = "MSSQL Server Adapter Stored Procedure",
                    Icon = "fa fa-windows",
                    Nav = false,
                    ModuleId = "viewmodels/adapter.sqlserver.sproc",
                    Role = "developers",
                    Settings = new JsRouteSetting(),
                    ShowWhenLoggedIn = true
                }};
                return list;
            }
        }
    }
}