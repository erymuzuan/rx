using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class EntityFormProvider : EntityDefinitionItemsProviders<EntityForm>
    {
        protected override string Icon => "fa fa-pencil-square-o";
        protected override string GetUrl(EntityForm item) => $"entity.form.designer/{item.Entity}/{item.Id}";
        protected override string GetName(EntityForm item) => item.Name;
        protected override string GetEntityDefinitionName(EntityForm item) => item.Entity;
    }
}