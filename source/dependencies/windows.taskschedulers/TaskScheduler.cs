using System;
using Humanizer;
using Microsoft.Win32.TaskScheduler;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WindowsTaskScheduler
{
    public class TaskScheduler : ITaskScheduler
    {
        public TaskScheduler(string executable)
        {
            this.Executable = executable;
        }
        public string Executable { get; set; }

        public System.Threading.Tasks.Task AddTaskAsync(DateTime dateTime, ScheduledActivityExecution info)
        {
            var path = this.GetPath(info);
            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Scheduled task for Delay Activity for " + info;

                // one time trigger
                td.Triggers.Add(new TimeTrigger(dateTime));
                var action = new ExecAction(this.Executable, string.Format("{0} {1}", info.ActivityId, info.InstanceId))
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName(this.Executable)
                };
                td.Actions.Add(action);

                ts.RootFolder.RegisterTaskDefinition(path, td);
            }
            return System.Threading.Tasks.Task.FromResult(0);

        }
        public System.Threading.Tasks.Task DeleteAsync(ScheduledActivityExecution info)
        {
            var path = this.GetPath(info);
            using (var ts = new TaskService())
            {
                ts.RootFolder.DeleteTask(path);
            }

            return System.Threading.Tasks.Task.FromResult(0);
        }

        private string GetPath(ScheduledActivityExecution info)
        {

            var guid = info.Name.Dehumanize() + "_" + info.ActivityId + "_" + info.InstanceId;
            var path = @"Bespoke\" + guid.Replace(" ", string.Empty);
            return path;
        }
    }
}
