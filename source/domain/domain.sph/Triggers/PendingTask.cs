namespace Bespoke.Sph.Domain
{
    public class PendingTask
    {
        public PendingTask(string id)
        {
            this.WorkflowId = id;
        }
        public PendingTask()
        {
            
        }
        public string WorkflowId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string WebId { get; set; }
        public string[] Correlations { get; set; }
        public string[] Performers { get; set; }
    }
}