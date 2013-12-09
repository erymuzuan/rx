using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Breakpoint : DomainObject
    {
        private Workflow m_wf;
        private WorkflowDefinition m_wd;

        public Breakpoint()
        {

        }

        public Breakpoint(Workflow wf, WorkflowDefinition wd)
        {
            m_wf = wf;
            m_wd = wd;
        }

        private TaskCompletionSource<bool> m_tcs;

        public Workflow Workflow
        {
            get { return m_wf; }
            set { m_wf = value; }
        }

        public WorkflowDefinition WorkflowDefinition
        {
            get { return m_wd; }
            set { m_wd = value; }
        }

        public void Break()
        {
            m_tcs = new TaskCompletionSource<bool>();
        }
        public void Continue()
        {
            m_tcs.SetResult(true);
        }
        public Task WaitAsync()
        {
            return m_tcs.Task;
        }

        public void Stop()
        {
            m_tcs.TrySetResult(true);
        }

        public void StepIn()
        {
            throw new System.NotImplementedException();
        }

        public void StepOut()
        {
            throw new System.NotImplementedException();
        }
        public void StepThrough()
        {
            m_tcs.TrySetResult(true);
            this.Operation = "StepThrough";
        }

        public string Operation { get; set; }
        public object EvaluateConsole(string console)
        {
            var script = ObjectBuilder.GetObject<IScriptEngine>();
            return script.Evaluate<object, Workflow>(console, Workflow);
        }
    }
}