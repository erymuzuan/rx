using System;
using System.ComponentModel.Composition;
using System.Linq;
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
            var item = this.Adapters.SingleOrDefault(x => x.Value.GetType() == d.GetType());
            if (!string.IsNullOrWhiteSpace(item?.Metadata.FontAwesomeIcon))
                return "fa fa-" + item.Metadata.FontAwesomeIcon;
            if (!string.IsNullOrWhiteSpace(item?.Metadata.BootstrapIcon))
                return "glyphicon glyphicon-" + item.Metadata.BootstrapIcon;
            if (!string.IsNullOrWhiteSpace(item?.Metadata.PngIcon))
                return item.Metadata.PngIcon;
            return "fa fa-database";

        }

        protected override string GetEditUrl(Adapter d)
        {
            var id = d.Id;

            var item = this.Adapters.SingleOrDefault(x => x.Value.GetType() == d.GetType());
            if (null != item)
                return item.Metadata.Route.ToEmptyString().Replace("/0", "/" + id);
            return string.Empty;
        }

        protected override string GetName(Adapter d) => d.Name;
    }
}