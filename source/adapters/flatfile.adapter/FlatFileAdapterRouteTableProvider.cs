using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class FlatFileAdapterRouteTableProvider : IRouteTableProvider
    {
        public string GetEditorViewModel(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.flatfile")
                return Properties.Resources.FlatFileAdapterJs;
            if (route.ModuleId == "viewmodels/adapter.flatfile.operation")
                return Properties.Resources.FlatFileOperationJs;
            if (route.ModuleId == "viewmodels/adapter.flatfile.wizard.delimiter")
                return Properties.Resources.DelimiterWizardJs;
            return null;
        }

        public string GetEditorView(JsRoute route)
        {
            if (route.ModuleId == "viewmodels/adapter.flatfile")
                return Properties.Resources.FlatFileAdapterHtml;
            if (route.ModuleId == "viewmodels/adapter.flatfile.operation")
                return Properties.Resources.FlatFileOperationHtml;
            if (route.ModuleId == "viewmodels/adapter.flatfile.wizard.delimiter")
                return Properties.Resources.DelimiterWizardHtml;
            return null;
        }

        public IEnumerable<JsRoute> Routes => new[]{
            new JsRoute
            {
                Caption = "Flat File Adapter",
                GroupName = "Adapter",
                IsAdminPage = true,
                Route = "adapter.flatfile/:id",
                Title = "Flat File Adapter",
                Icon = "fa fa-html5",
                Nav = false,
                ModuleId = "viewmodels/adapter.flatfile",
                Role = "developers",
                Settings = new JsRouteSetting(),
                ShowWhenLoggedIn = true,
                StartPageRoute = null
            },
            new JsRoute
            {
                Caption = "Web page operation",
                GroupName = "Adapter",
                IsAdminPage = false,
                Route = "adapter.flatfile.operation/:id/:uuid",
                ModuleId = "viewmodels/adapter.flatfile.operation",
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