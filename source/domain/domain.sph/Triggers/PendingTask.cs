namespace Bespoke.Sph.Domain
{
    public class PendingTask
    {
        public PendingTask(int id)
        {
            this.WorkflowId = id;
        }
        public PendingTask()
        {
            
        }
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string WebId { get; set; }
        public string[] Correlations { get; set; }
        public string[] Performers { get; set; }
    }
}