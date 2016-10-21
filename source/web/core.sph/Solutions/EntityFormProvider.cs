using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class EntityFormProvider : EntityDefinitionItemsProviders<EntityForm>
    {
        protected override string Icon => "bowtie-icon  bowtie-storyboard";
        protected override string GetUrl(EntityForm item) => $"entity.form.designer/{item.EntityDefinitionId}/{item.Id}";
        protected override string GetName(EntityForm item) => item.Name;
        protected override string GetEntityDefinitionName(EntityForm item) => item.Entity;
    }
}