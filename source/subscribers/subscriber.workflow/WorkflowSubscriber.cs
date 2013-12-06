using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowSubscriber : Subscriber<Workflow>
    {
        public override string QueueName
        {
            get { return "workflow_execution"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Workflow.*.Execute" }; }
        }


        protected async override Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", item.WorkflowDefinitionId, item.Version));

            if (item.State == "Completed") return;

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromXml<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;

            // get current activity
            dynamic headers = header;
            var initiator = wd.ActivityCollection.Single(a => a.IsInitiator);
            string activityId = headers.ActivityWebId;
            

            if (initiator.WebId == activityId)
            {
                this.WriteMessage("started");
            }
            this.WriteMessage("Running {0}", item.GetCurrentActivity());

            // debugger
            var breakpoint = this.GetBreakpoint(item);
            if (null != breakpoint)
            {
                await this.SendLocalsAsync(item);
                await breakpoint.WaitAsync();
            }

            var result = await item.ExecuteAsync();
            this.WriteMessage(result);

        }

        private Task SendLocalsAsync(Workflow item)
        {
            throw new System.NotImplementedException();
        }

        private Breakpoint GetBreakpoint(Workflow item)
        {
            throw new System.NotImplementedException();
        }


        protected override void OnStart()
        {
            var listener = new TcpListener(IPAddress.Loopback, 8181);
            listener.Start();
            using (var client = listener.AcceptTcpClient())
            using (var stream = client.GetStream())
            using (var reader = new StreamReader(stream))
            using (var writer = new StreamWriter(stream))
            {
                writer.WriteLine("HTTP/1.1 101 Web Socket Protocol Handshake");
                writer.WriteLine("Upgrade: WebSocket");
                writer.WriteLine("Connection: Upgrade");
                writer.WriteLine("WebSocket-Origin: http://localhost:4436");
                writer.WriteLine("WebSocket-Location: ws://localhost:50525/debugger");
                writer.WriteLine("");
            }
            listener.Stop();
        }
    }
}
