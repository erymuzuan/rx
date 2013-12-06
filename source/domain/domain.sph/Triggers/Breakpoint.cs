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
    }
}