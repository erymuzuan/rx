using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class TriggerProvider : EntityDefinitionItemsProviders<Trigger>
    {
        protected override string Icon => "fa fa-bolt";
        protected override string GetUrl(Trigger item) => $"trigger.setup/{item.Id}";
        protected override string GetName(Trigger item) => item.Name;
        protected override string GetEntityDefinitionName(Trigger item) => item.Entity;

    }
}