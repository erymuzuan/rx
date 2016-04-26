using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class EntityViewProvider : EntityDefinitionItemsProviders<EntityView>
    {
        protected override string Icon => "fa fa-list-ul";
        protected override string GetUrl(EntityView item) => $"entity.view.designer/{item.Id}";
        protected override string GetName(EntityView item) => item.Name;
        protected override string GetEntityDefinitionName(EntityView item) => item.Entity;
    }
}