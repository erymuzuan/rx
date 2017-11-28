using System;
using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.WebApi
{
    [Export(typeof(IDeveloperService))]
    public class DeveloperService : IDeveloperService
    {
        private static bool m_initialized;
        public static void Init()
        {
            if (m_initialized) return;
            m_initialized = true;

            var ds = new DeveloperService();
            ObjectBuilder.ComposeMefCatalog(ds);
            ObjectBuilder.AddCacheList<IDeveloperService>(ds);
        }

        [ImportMany(typeof(IBuildDiagnostics))]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        [ImportMany(typeof(IProjectBuilder))]
        public IProjectBuilder[] ProjectBuilders{ get; set; }

        [ImportMany(typeof(IProjectDeployer))]
        public IProjectDeployer[] ProjectDeployers{ get; set; }


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

        [ImportMany("ReceiveLocationDesigner", typeof(ReceiveLocation), AllowRecomposition = true)]
        public Lazy<ReceiveLocation, IDesignerMetadata>[] ReceiveLocationOptions { get; set; }


        [ImportMany(typeof(ControllerAction))]
        public ControllerAction[] ActionCodeGenerators { get; set; }
    }
}