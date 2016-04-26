using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class WorkflowFormProviders : SourceAssetProviders<WorkflowForm>
    {
        protected override string Icon => "fa fa-wpforms";
        protected override string GetIcon(WorkflowForm d) => this.Icon;
        protected override string GetEditUrl(WorkflowForm d) => $"workflow.form.designer/{d.WorkflowDefinitionId}/{d.Id}";
        protected override string GetName(WorkflowForm d) => d.Name;
    }
}