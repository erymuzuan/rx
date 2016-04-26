using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class ValueObjectDefinitionProviders : SourceAssetProviders<ValueObjectDefinition>
    {
        protected override string Icon => "fa fa-object-ungroup";
        protected override string GetIcon(ValueObjectDefinition d) => this.Icon;
        protected override string GetEditUrl(ValueObjectDefinition d) => $"value.object.details/{d.Id}";
        protected override string GetName(ValueObjectDefinition d) => d.Name;
    }
}