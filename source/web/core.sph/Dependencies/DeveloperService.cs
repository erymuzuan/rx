using System;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Web.Dependencies
{
    [Export]
    public class DeveloperService
    {
        private static bool m_initialized;
        public static void Init()
        {
            if(m_initialized)return;
            m_initialized = true;

            var ds = new DeveloperService();
            ObjectBuilder.ComposeMefCatalog(ds);
            ObjectBuilder.AddCacheList(ds);
        }


        [ImportMany("ActivityDesigner", typeof(Activity), AllowRecomposition = true)]
        public Lazy<Activity, IDesignerMetadata>[] ActivityOptions { get; set; }

        [ImportMany("FormDesigner", typeof(FormElement), AllowRecomposition = true)]
        public Lazy<FormElement, IDesignerMetadata>[] ToolboxItems { get; set; }

        [ImportMany("FunctoidDesigner", typeof(Functoid), AllowRecomposition = true)]
        public Lazy<Functoid, IDesignerMetadata>[] Functoids { get; set; }

        [ImportMany(typeof(CustomAction), AllowRecomposition = true)]
        public Lazy<CustomAction, IDesignerMetadata>[] ActionOptions { get; set; }

        [ImportMany("AdapterDesigner", typeof(Adapter), AllowRecomposition = true)]
        public Lazy<Adapter, IDesignerMetadata>[] Adapters { get; set; }
    }
}