using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Microsoft.Win32.TaskScheduler;
using Task = System.Threading.Tasks.Task;
using Bespoke.Sph.WorkflowTriggerSubscriptions;

namespace Bespoke.Sph.ReportDeliverySubscriptions
{
    public class ReportDeliverySubscriber : Subscriber<ReportDelivery>
    {
        private readonly string m_executable;
        public ReportDeliverySubscriber(string executable)
        {
            m_executable = executable;
        }

        public ReportDeliverySubscriber()
        {
            
        }

        public override string QueueName => "report_delivery_scheduler";
        public override string[] RoutingKeys => new[] { "ReportDelivery.#" };

        protected override Task ProcessMessage(ReportDelivery item, MessageHeaders header)
        {
            var emptyTask = Task.FromResult(0);
            var path = GetPath(item);

            // delete if exist
            Delete(path);
            if (!item.IsActive) return emptyTask;

            this.WriteMessage("Creating scheduler for " + item.Title);
            var scheduledTask = item.IntervalScheduleCollection.Where(t => t.IsEnabled);

            using (var ts = new TaskService())
            {
                var td = ts.NewTask();
                td.RegistrationInfo.Description = "Scheduled trigger for " + item.Title;
                foreach (var t in scheduledTask)
                {
                    var trigger = t.GeTrigger();
                    td.Triggers.Add(trigger);
                }
                var action = new ExecAction(this.Executable, $"{item.Id}")
                {
                    WorkingDirectory = System.IO.Path.GetDirectoryName(this.Executable)
                };
                td.Actions.Add(action);
                ts.RootFolder.RegisterTaskDefinition(path, td);
                return Task.FromResult(0);
            }
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

        public string Executable => string.IsNullOrWhiteSpace(m_executable) ? ConfigurationManager.ReportDeliveryExecutable : m_executable;

        private static string GetPath(ReportDelivery item)
        {
            var guid = $"rd_{item.Id}_{item.ReportDefinitionId}";
            var path = @"Bespoke\" + guid.Replace(" ", string.Empty);
            return path;

        }

        public void Test(ReportDelivery wd, MessageHeaders headers)
        {
            this.ProcessMessage(wd, headers).Wait();
        }
    }
}
