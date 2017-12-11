using System;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Domain
{
    public interface IDeveloperService
    {
        IBuildDiagnostics[] BuildDiagnostics { get; set; }
        Lazy<Activity, IDesignerMetadata>[] ActivityOptions { get; set; }
        Lazy<FormElement, IDesignerMetadata>[] ToolboxItems { get; set; }
        Lazy<Functoid, IDesignerMetadata>[] Functoids { get; set; }
        Lazy<CustomAction, IDesignerMetadata>[] ActionOptions { get; set; }
        Lazy<Adapter, IDesignerMetadata>[] Adapters { get; set; }
        Lazy<ReceiveLocation, IDesignerMetadata>[] ReceiveLocationOptions { get; set; }
        ControllerAction[] ActionCodeGenerators { get; set; }
        IProjectBuilder[] ProjectBuilders { get; set; }
        IProjectDeployer[] ProjectDeployers { get; set; }
    }
}
