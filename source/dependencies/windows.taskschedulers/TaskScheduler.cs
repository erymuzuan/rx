using System;
using Microsoft.Win32.TaskScheduler;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WindowsTaskScheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        public string Executable { get; set; }

        public async System.Threading.Tasks.Task AddTaskAsync(DateTime dateTime, ScheduledActivityExecution info)
        {
            var guid = info.Name + "_" + info.ActivityId + "_" + info.InstanceId;
            var path = @"Bespoke\" + guid;
            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Scheduled task for Delay Activity for " + info;

                // one time trigger
                td.Triggers.Add(new TimeTrigger(dateTime));

                td.Actions.Add(new ExecAction(this.Executable, string.Format("{0} {1}", info.ActivityId, info.InstanceId)));

                ts.RootFolder.RegisterTaskDefinition(path, td);
            }
            await System.Threading.Tasks.Task.Delay(100);

        }
        public async System.Threading.Tasks.Task DeleteAsync(ScheduledActivityExecution info)
        {
            var guid = info.Name + "_" + info.ActivityId + "_" + info.InstanceId;
            var path = @"Bespoke\-" + guid;
            using (var ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(path);
            }
            await System.Threading.Tasks.Task.Delay(100);
        }
    }
}
