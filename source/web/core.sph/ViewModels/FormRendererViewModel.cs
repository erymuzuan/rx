using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class FormRendererViewModel
    {
        public WorkflowForm WorkflowForm { get; set; }
        public ReceiveActivity Activity { get; set; }

        public FormRendererViewModel(EntityDefinition ed, EntityForm form)
        {
            this.EntityDefinition = ed;
            this.Form = form;
        }
        public FormRendererViewModel(WorkflowDefinition wd, WorkflowForm workflowForm, ReceiveActivity activity)
        {
            WorkflowForm = workflowForm;
            Activity = activity;
        }

        public EntityForm Form { get; set; }
        public EntityDefinition EntityDefinition { get; set; }
        public WorkflowDefinition WorkflowDefinition{ get; set; }
    }
}