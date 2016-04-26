using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class WorkflowDefinitionProviders : SourceAssetProviders<WorkflowDefinition>
    {
        protected override string Icon => "fa fa-code-fork";
        protected override string GetIcon(WorkflowDefinition item) => this.Icon;
        protected override string GetEditUrl(WorkflowDefinition d) => $"workflow.definition.visual/{d.Id}";
        protected override string GetName(WorkflowDefinition item) => item.Name;
    }
}