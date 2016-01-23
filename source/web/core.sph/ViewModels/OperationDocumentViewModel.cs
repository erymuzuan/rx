using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class OperationDocumentViewModel
    {
        public EntityDefinition Ed { get; set; }

        public OperationDocumentViewModel(EntityDefinition ed)
        {
            Ed = ed;
        }
    }
}