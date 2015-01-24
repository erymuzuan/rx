using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.adapter.compiler
{
    public class AdapterCompilerSubscriber : Subscriber<Adapter>
    {
        public override string QueueName
        {
            get { return "adapter_compiler"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "SqlServerAdapter.#.Publish","Adapter.#.Publish" }; }
        }

        protected  async override Task ProcessMessage(Adapter adapter, MessageHeaders header)
        {

            this.WriteMessage("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            this.WriteMessage("Publishing {0} in progress....", adapter.Name);

            // NOTE : copy dlls, this will cause the appdomain to unload and we want it happend
            // after the Ack to the broker
            await adapter.OpenAsync();
            var result = await adapter.CompileAsync();
            ThreadPool.QueueUserWorkItem(Deploy, result);
        
        }

        private static void Deploy(object obj)
        {
            Thread.Sleep(1000);

            var result = (WorkflowCompilerResult)obj;
            if (result.Result)
            {
                var output = Path.GetFileNameWithoutExtension(result.Output);

                File.Copy(result.Output, ConfigurationManager.SchedulerPath + "\\" + output + ".dll", true);
                File.Copy(result.Output.Replace(".dll", ".pdb"), ConfigurationManager.SchedulerPath + "\\" + output + ".pdb", true);

                File.Copy(result.Output, string.Format("{0}\\{1}.dll", ConfigurationManager.SubscriberPath, output), true);
                File.Copy(result.Output.Replace(".dll", ".pdb"), ConfigurationManager.SubscriberPath + "\\" + output + ".pdb", true);

                File.Copy(result.Output, ConfigurationManager.WebPath + @"\bin\" + output + ".dll", true);
                File.Copy(result.Output.Replace(".dll", ".pdb"), ConfigurationManager.WebPath + @"\bin\" + output + ".pdb", true);
            }
        }
    }
}