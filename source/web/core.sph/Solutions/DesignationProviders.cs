using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class DesignationProviders : SourceAssetProviders<Designation>
    {
        protected override string Icon => "fa fa-users";
        protected override string GetIcon(Designation d) => this.Icon;
        protected override string GetEditUrl(Designation d) => $"role.settings/{d.Id}";
        protected override string GetName(Designation d) => d.Name;
    }
}