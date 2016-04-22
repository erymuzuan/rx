namespace Bespoke.Sph.Domain
{
    public class PendingTask
    {
        public PendingTask(string workflow, string worflowDefinition)
        {
            this.WorkflowId = workflow;
            this.WorkflowDefinitionId = worflowDefinition;
        }
        public PendingTask()
        {
            
        }
        public string WorkflowId { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public string ActivityName { get; set; }
        public string Type { get; set; }
        public string ActivityWebId { get; set; }
        public string[] Correlations { get; set; }
    }
}