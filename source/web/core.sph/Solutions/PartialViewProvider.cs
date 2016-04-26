using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class PartialViewProvider : EntityDefinitionItemsProviders<PartialView>
    {
        protected override string Icon => "fa fa-file-o";
        protected override string GetUrl(PartialView item) =>$"partial.view.designer/{item.Entity}/{item.Id}";
    }
}