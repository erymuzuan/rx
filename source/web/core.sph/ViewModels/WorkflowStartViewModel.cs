using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class WorkflowStartViewModel
    {
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public ScreenActivity Screen { get; set; }
    }
}