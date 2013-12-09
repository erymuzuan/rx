using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class DebuggerModel
    {
        public string Operation { get; set; }
        public Breakpoint Breakpoint { get; set; }
        public string Console { get; set; }
    }
}