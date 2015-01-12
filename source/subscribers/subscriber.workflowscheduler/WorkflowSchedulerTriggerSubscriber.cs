using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Microsoft.Win32.TaskScheduler;
using Task = System.Threading.Tasks.Task;

namespace Bespoke.Sph.WorkflowTriggerSubscriptions
{
    public class WorkflowSchedulerTriggerSubscriber : Subscriber<WorkflowDefinition>
    {
        private readonly string m_executable;

        public WorkflowSchedulerTriggerSubscriber()
        {

        }

        public WorkflowSchedulerTriggerSubscriber(string executable)
        {
            m_executable = executable;
        }

        public override string QueueName
        {
            get { return "workflow_scheduler_trigger"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "WorkflowDefinition.#.Publish" }; }
        }

        protected override Task ProcessMessage(WorkflowDefinition item, MessageHeaders header)
        {
            var emptyTask = Task.FromResult(0);

            var start = item.ActivityCollection.Single(a => a.IsInitiator) as ScheduledTriggerActivity;
            if (null == start) return emptyTask;

            var scheduledTask = start.IntervalScheduleCollection.Where(s => s.IsEnabled).ToArray();
            var path = this.GetPath(item);

            // delete if exist
            this.Delete(path);
            if (!item.IsActive) return emptyTask;


            this.WriteMessage("Creating scheduler for " + item.Name);
            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Scheduled trigger for " + item.Name;
                foreach (var t in scheduledTask)
                {
                    var trigger = t.GeTrigger();
                    td.Triggers.Add(trigger);
                }
                var action = new ExecAction(this.Executable, string.Format("{0} {1}", start.WebId, item.Id))
                            {
                                WorkingDirectory = System.IO.Path.GetDirectoryName(this.Executable)
                            };
                td.Actions.Add(action);
                ts.RootFolder.RegisterTaskDefinition(path, td);
                return Task.FromResult(0);
            }
        }

        private void Delete(string path)
        {
            using (var ts = new TaskService())
            {
                var td = ts.GetTask(path);
                if (null != td)
                    ts.RootFolder.DeleteTask(path);
            }

        }

        public string Executable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_executable))
                    return ConfigurationManager.ScheduledTriggerActivityExecutable;

                return m_executable;
            }
        }

        private string GetPath(WorkflowDefinition item)
        {
            var guid = "START_" + item.WorkflowTypeName;
            var path = @"Bespoke\" + guid.Replace(" ", string.Empty);
            return path;

        }

        public async Task Test(WorkflowDefinition wd, MessageHeaders headers)
        {
            await this.ProcessMessage(wd, headers).ConfigureAwait(false);
        }
    }
}
