using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class Breakpoint : DomainObject
    {
        private TaskCompletionSource<bool> m_tcs;
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
    }
}