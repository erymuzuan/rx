using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class MySqlServerRouteProvider : IRouteTableProvider
    {
        public string GetEditorViewModel(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.mysql")
                return Properties.Resources.adapter_mysql_js;
            if (route.ModuleId == "viewmodels/adapter.mysql.sproc")
                return Properties.Resources.adapter_mysql_sproc_js;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.mysql")
                return Properties.Resources.adapter_mysql_html;
            if (route.ModuleId == "viewmodels/adapter.mysql.sproc")
                return Properties.Resources.adapter_mysql_sproc_html;
            return null;
        }

        public IEnumerable<JsRoute> Routes => new List<JsRoute> {new JsRoute
        {
            Caption = "MySql Server Adapter",
            GroupName = "developers",
            IsAdminPage = true,
            Route = "adapter.mysql/:id",
            Title = "MySql Server Adapter",
            Icon = "fa fa-windows",
            Nav = false,
            ModuleId = "viewmodels/adapter.mysql",
            Role = "developers",
            Settings = new JsRouteSetting(),
            ShowWhenLoggedIn = true,
            StartPageRoute = null
        },
            new JsRoute
            {
                Caption = "MySQl Server Adapter",
                GroupName = "developers",
                IsAdminPage = true,
                Route = "adapter.mysql.sproc/:id/:sproc",
                Title = "MySql Server Adapter Stored Procedure",
                Icon = "fa fa-windows",
                Nav = false,
                ModuleId = "viewmodels/adapter.mysql.sproc",
                Role = "developers",
                Settings = new JsRouteSetting(),
                ShowWhenLoggedIn = true
            }};
    }
}