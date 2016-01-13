using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;

namespace Bespoke.Sph.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        static TemplateFormViewModel ()
        {
            DeveloperService.Init();
        }


        public ObjectCollection<FormElement> FormElements
        {
            get
            {
                var ds = ObjectBuilder.GetObject<DeveloperService>();
                var items = ds.ToolboxItems.OrderBy(x => x.Metadata.Order).Select(x => x.Value);
                return items.ToObjectCollection();
            }
        }

        public bool IsImportVisible { get; set; }
    }
}