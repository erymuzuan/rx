using System;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class CircuitBreaker
    {
        private readonly int m_exceptionsBeforeBreaking;
        private readonly TimeSpan m_breakDuration;
        private int m_exceptionsCount;
        private TaskCompletionSource<object> m_circuitClosedTcs;

        public CircuitBreaker(int exceptionsBeforeBreaking, TimeSpan breakDuration)
        {
            m_exceptionsBeforeBreaking = exceptionsBeforeBreaking;
            m_breakDuration = breakDuration;
        }

        public bool IsOpen => m_exceptionsCount >= m_exceptionsBeforeBreaking;

        public Task<object> WaitToCloseAsync()
        {
            if (!IsOpen || null == m_circuitClosedTcs)
                throw new InvalidOperationException("Circuit is not closed");

            return m_circuitClosedTcs.Task;
        }

        public T GetResult<T>(PolicyResult<T> pr)
        {
            if (this.IsOpen)
                throw new InvalidOperationException("You cannot get result while the circuit is open");
            if (null == pr.FinalException)
                return pr.Result;

            Interlocked.Increment(ref m_exceptionsCount);
            if (IsOpen)
            {
                m_circuitClosedTcs = new TaskCompletionSource<object>();
                var timer = new System.Timers.Timer
                {
                    Interval = m_breakDuration.TotalMilliseconds,
                    Enabled = true
                };
                timer.Elapsed += (s, e) =>
                {
                    m_circuitClosedTcs.SetResult(e);
                    while (m_exceptionsCount > 0)
                    {
                        Interlocked.Decrement(ref m_exceptionsCount);
                    }
                    m_circuitClosedTcs = null;
                };
            }

            throw pr.FinalException;
        }
    }
}