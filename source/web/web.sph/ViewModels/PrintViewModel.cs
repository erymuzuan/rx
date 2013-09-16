using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class PrintViewModel
    {
        public string Name { set; get; }
        public Entity Item { get; set; }
        public FormDesign FormDesign { get; set; }
    }
}