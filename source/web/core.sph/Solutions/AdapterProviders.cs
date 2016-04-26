using System;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class AdapterProviders : SourceAssetProviders<Adapter>
    {
        [ImportMany("AdapterDesigner", typeof(Adapter), AllowRecomposition = true)]
        public Lazy<Adapter, IDesignerMetadata>[] Adapters { get; set; }

        protected override string Icon => "fa fa-puzzle-piece";
        protected override string GetIcon(Adapter d)
        {
            var type = d.GetType().GetShortAssemblyQualifiedName();
            switch (type)
            {
                case "Bespoke.Sph.Integrations.Adapters.MySqlAdapter, mysql.adapter":
                    return "fa fa-database";
                case "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter":
                    return "fa fa-windows";
                case "Bespoke.Sph.Integrations.Adapters.HttpAdapter, http.adapter":
                    return "fa fa-html5";
                case "Bespoke.Sph.Integrations.Adapters.OrcaleAdapter, oracle.adapter":
                    return "fa fa-database";
            }

            return "fa fa-puzzle-piece";

        }

        protected override string GetEditUrl(Adapter d)
        {
            var id = d.Id;
            var type = d.GetType().GetShortAssemblyQualifiedName();
            if (type == "Bespoke.Sph.Integrations.Adapters.MySqlAdapter, mysql.adapter")
                return $"adapter.mysql/{id}";
            if (type == "Bespoke.Sph.Integrations.Adapters.SqlServerAdapter, sqlserver.adapter")
                return $"adapter.sqlserver/{id}";
            if (type == "Bespoke.Sph.Integrations.Adapters.HttpAdapter, http.adapter")
                return $"adapter.http/{id}";
            if (type == "Bespoke.Sph.Integrations.Adapters.OrcaleAdapter, oracle.adapter")
                return $"adapter.oracle/{id}";
            return string.Empty;
        }

        protected override string GetName(Adapter d) => d.Name;
    }
}