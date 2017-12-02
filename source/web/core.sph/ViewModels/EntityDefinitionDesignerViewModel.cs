using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class EntityDefinitionDesignerViewModel
    {
        public EntityDefinition Item { get; set; }

        public ObjectCollection<AttachedProperty> AttachedProperties { get; } = new ObjectCollection<AttachedProperty>();
    }
}