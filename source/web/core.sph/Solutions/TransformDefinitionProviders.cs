using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class TransformDefinitionProviders : SourceAssetProviders<TransformDefinition>
    {
        protected override string Icon => "fa fa-random";
        protected override string GetIcon(TransformDefinition d) => this.Icon;
        protected override string GetEditUrl(TransformDefinition d) => $"transform.definition.edit/{d.Id}";
        protected override string GetName(TransformDefinition d) => d.Name;
    }
}