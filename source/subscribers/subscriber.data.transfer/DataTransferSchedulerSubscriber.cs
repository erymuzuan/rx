using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.WorkflowTriggerSubscriptions;
using Microsoft.Win32.TaskScheduler;
using Task = System.Threading.Tasks.Task;

namespace subscriber.data.transfer
{
    public class DataTransferSchedulerSubscriber : Subscriber<DataTransferDefinition>
    {
        public DataTransferSchedulerSubscriber() { }
        private readonly string m_executable;
        public DataTransferSchedulerSubscriber(string executable)
        {
            this.m_executable = executable;
        }
        public override string QueueName => "DataTransferSchedulerSubscriber";
        public override string[] RoutingKeys => new[] { "DataTransferDefinition.#.#" };
        protected override Task ProcessMessage(DataTransferDefinition item, MessageHeaders header)
        {
            var scheduledTask = item.IntervalScheduleCollection;
            for (var i = 0; i < 10; i++)
            {
                var path = $@"Bespoke\{item.Id}_{i}";
                Delete(path);
            }

            var instance = 0;
            foreach (var t in scheduledTask)
            {
                var metadata = item.ScheduledDataTransferCollection.Single(x => x.ScheduleWebId == t.WebId);
                instance++;
                var path = $@"Bespoke\{item.Id}_{instance}";
                Delete(path);
                var trigger = t.GeTrigger();
                using (var ts = new TaskService())
                {
                    var td = ts.NewTask();
                    td.RegistrationInfo.Description = "Scheduled data transfer for " + item.Name;

                    td.Triggers.Add(trigger);
                    var notificationOnError = metadata.NotifyOnError ? " /notificationOnError" : "";
                    var notificationOnSuccess = metadata.NotifyOnSuccess ? " /notificationOnSuccess" : "";
                    var truncateData = metadata.TruncateData ? " /truncateData" : "";

                    var action = new ExecAction(this.Executable, $"{item.Id}{notificationOnError}{notificationOnSuccess}{truncateData}")
                    {
                        WorkingDirectory = System.IO.Path.GetDirectoryName(this.Executable)
                    };
                    td.Actions.Add(action);
                    ts.RootFolder.RegisterTaskDefinition(path, td);
                }
            }
            return Task.FromResult(0);
        }

        private static void Delete(string path)
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
                    return ConfigurationManager.ScheduledDataTransferExecutable;

                return m_executable;
            }
        }
    }
}