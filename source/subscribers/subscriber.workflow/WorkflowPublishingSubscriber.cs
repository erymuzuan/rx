using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowPublishingSubscriber : Subscriber<WorkflowDefinition>
    {
        public override string QueueName
        {
            get { return "workflow_definition_publish"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "WorkflowDefinition.#.Publish" }; }
        }

        protected override Task ProcessMessage(WorkflowDefinition item, MessageHeaders header)
        {
            // NOTE : copy dlls, this will cause the appdomain to unload and we want it happend
            // after the Ack to the broker
            ThreadPool.QueueUserWorkItem( Deploy, item);
            return Task.FromResult(0);
        }

        private static void Deploy(object obj)
        {
            Thread.Sleep(1000);
            var item = (WorkflowDefinition) obj;
            var dll = string.Format("workflows.{0}.{1}.dll", item.WorkflowDefinitionId, item.Version);
            var pdb = string.Format("workflows.{0}.{1}.pdb", item.WorkflowDefinitionId, item.Version);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, ConfigurationManager.WebPath + @"\bin\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.WebPath + @"\bin\" + pdb, true);


            File.Copy(dllFullPath, ConfigurationManager.SubscriberPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SubscriberPath + @"\" + pdb, true);
            
            File.Copy(dllFullPath, ConfigurationManager.SchedulerPath + @"\" + dll, true);
            File.Copy(pdbFullPath, ConfigurationManager.SchedulerPath + @"\" + pdb, true);
        }
    }
}