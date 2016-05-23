using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class DataTransferDefinitionProviders : SourceAssetProviders<DataTransferDefinition>
    {
        protected override string Icon => "fa fa-database";
        protected override string GetIcon(DataTransferDefinition d) => this.Icon;
        protected override string GetEditUrl(DataTransferDefinition d) => $"data.import/{d.Id}";
        protected override string GetName(DataTransferDefinition d) => d.Name;
    }
}