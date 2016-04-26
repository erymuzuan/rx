using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class ReportDefinitionProviders : SourceAssetProviders<ReportDefinition>
    {
        protected override string Icon => "fa fa-file-word-o";
        protected override string GetIcon(ReportDefinition d) => this.Icon;
        protected override string GetEditUrl(ReportDefinition d) => $"reportdefinition.edit/{d.Id}";
        protected override string GetName(ReportDefinition d) => d.Title;
    }
}