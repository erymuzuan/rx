using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bespoke.Sph.Domain.Api
{
    public class AdapterDesigner
    {
        [ImportMany("AdapterDesigner", typeof(Adapter), AllowRecomposition = true)]
        public Lazy<Adapter, IDesignerMetadata>[] AdaptersMetadata { get; set; }

        public IEnumerable<JsRoute> GetRoutes()
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var routes = from a in AdaptersMetadata
                         where null != a.Metadata.RouteTableProvider
                         let b = Activator.CreateInstance(a.Metadata.RouteTableProvider) as IRouteTableProvider
                         where null != b
                         select b.Routes;
            return routes.SelectMany(x => x.ToArray());
        }

    }
}
