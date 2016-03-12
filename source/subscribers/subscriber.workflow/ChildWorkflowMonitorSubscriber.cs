using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class ChildWorkflowMonitorSubscriber : Subscriber<Workflow>
    {
        public override string QueueName => "child_workflow_monitor";
        public override string[] RoutingKeys => new[] { "Workflow.Changed.#" };
        protected override async Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            if (item.State != "Completed") return;
            var parent = await item.GetParentWorkflowAsync();
            if (null == parent) return;

            var ca = parent.GetActivity<ChildWorkflowActivity>(parent.ParentActivity);
            foreach (var @var in ca.ExecutedPropertyMappingCollection)
            {
                // now extract the value
                Console.WriteLine($@"{@var.Source} ---> {@var.Destination}");
                var source = GetValue(item, @var.Source);
                this.SetValue(parent, @var.Destination, source);
            }

            await parent.ExecuteAsync(parent.ParentActivity);
        }

        private object GetValue(Workflow wf, string sourcePath)
        {
            var path = sourcePath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var type = wf.GetType();
            var prop = type.GetProperty(path[0]);
            if (path.Length == 1)
            {
                return prop.GetValue(wf);
            }

            object dd = wf;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var pname = path[i];
                prop = dd.GetType().GetProperty(pname);
                dd = prop.GetValue(dd);
            }
            prop = dd.GetType().GetProperty(path.Last());
            return prop.GetValue(dd);
            
        }

        private void SetValue(Workflow wf, string destinationPath, object val)
        {
            var type = wf.GetType();
            var path = destinationPath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            var prop = type.GetProperty(path[0]);
            if (path.Length == 1)
            {
                prop.SetValue(wf, val);
                return;
            }

            object dd = wf;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var pname = path[i];
                prop = dd.GetType().GetProperty(pname);
                dd = prop.GetValue(dd);
            }
            prop = dd.GetType().GetProperty(path.Last());
            prop.SetValue(dd, val);

        }

    }
}