using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class SpaceDetailViewModel
    {
        public SpaceTemplate Template { get; set; }

        public ApplicationTemplate[] ApplicationTemplates { get; set; }
    }
}