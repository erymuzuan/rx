using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Web.Dependencies
{
    public interface IDeveloperService
    {
        IBuildDiagnostics[] BuildDiagnostics { get; set; }
        Lazy<Activity, IDesignerMetadata>[] ActivityOptions { get; set; }
        Lazy<FormElement, IDesignerMetadata>[] ToolboxItems { get; set; }
        Lazy<Functoid, IDesignerMetadata>[] Functoids { get; set; }
        Lazy<CustomAction, IDesignerMetadata>[] ActionOptions { get; set; }
        Lazy<Adapter, IDesignerMetadata>[] Adapters { get; set; }
    }
}