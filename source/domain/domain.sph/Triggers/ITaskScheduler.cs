using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ITaskScheduler
    {
        Task AddTaskAsync(DateTime dateTime, ScheduledActivityExecution info);
        Task DeleteAsync(ScheduledActivityExecution info);
    }
}