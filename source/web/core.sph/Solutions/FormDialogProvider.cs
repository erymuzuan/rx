using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class FormDialogProvider : EntityDefinitionItemsProviders<FormDialog>
    {
        protected override string Icon => "fa fa-book";
        protected override string GetUrl(FormDialog item) => $"form.dialog.designer/{item.Entity}/{item.Id}";
        protected override string GetName(FormDialog item) => item.Title;
        protected override string GetEntityDefinitionName(FormDialog item) => item.Entity;
    }
}