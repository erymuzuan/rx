using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;

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
            this.CurrentBreakpoint = this.GetBreakpoint(item);
            if (null != this.CurrentBreakpoint)
            {
                this.WriteMessage("OOOOO -+-+-+ " + this.CurrentBreakpoint.ActivityWebId);
                await this.SendLocalsAsync(item);
                this.CurrentBreakpoint.Break();
                await this.CurrentBreakpoint.WaitAsync();
            }

            var result = await item.ExecuteAsync();
            this.WriteMessage(result);

        }

        public Breakpoint CurrentBreakpoint { get; set; }

        private Task SendLocalsAsync(Workflow item)
        {
            var model = new
            {
                Breakpoint = this.CurrentBreakpoint,
                Data = item
            };

            m_appServer.GetAllSessions().ToList().ForEach(e => e.Send(model.ToJsonString()));
            return Task.FromResult(0);
        }

        private Breakpoint GetBreakpoint(Workflow item)
        {
            return this.BreakpointCollection
                .Where(b => b.WorkflowDefinitionId == item.WorkflowDefinitionId)
                .Where(b => b.IsEnabled)
                .SingleOrDefault(b => b.ActivityWebId == item.CurrentActivityWebId);
        }

        private readonly ObjectCollection<Breakpoint> m_breakpointCollection = new ObjectCollection<Breakpoint>();
        public ObjectCollection<Breakpoint> BreakpointCollection
        {
            get { return m_breakpointCollection; }
        }

        private WebSocketServer m_appServer;
        protected override void OnStart()
        {
            m_appServer = new WebSocketServer();
            if (!m_appServer.Setup(50518))
            {
                this.WriteError(new Exception("Failed to setup WebSocket Server!"));
                Console.ReadKey();
                return;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;
            m_appServer.NewDataReceived += m_appServer_NewDataReceived;
            if (!m_appServer.Start())
            {
                this.WriteError(new Exception("Failed to start websocket server!"));
                return;
            }
            this.WriteMessage("The WebSocket server started successfully!");
        }

        void m_appServer_NewDataReceived(WebSocketSession session, byte[] value)
        {
            this.WriteMessage("New data received");
        }

        private void NewMessageReceived(WebSocketSession session, string value)
        {
            // TODO : receive different operations such as add breakpoint, remove, enabled, continue , step into , step out and step throu
            this.WriteMessage("New message received");
            var model = value.DeserializeFromJson<DebuggerModel>();
            if (null == model) return;
            if (null != model.Breakpoint)
            {
                this.WriteMessage("Receieve break point " + model.Breakpoint.ActivityWebId);
                var bp = this.BreakpointCollection.SingleOrDefault(b => b.ActivityWebId == model.Breakpoint.ActivityWebId);
                if (model.Operation == "AddBreakpoint")
                {
                    if (null != bp)
                        this.BreakpointCollection.Replace(bp, model.Breakpoint);
                    else
                        this.BreakpointCollection.Add(model.Breakpoint);
                }
                if (model.Operation == "RemoveBreakpoint")
                {
                    this.BreakpointCollection.Remove(bp);
                }
            }

            if (model.Operation == "Continue" && null != this.CurrentBreakpoint)
            {
                this.CurrentBreakpoint.Continue();
            }
        }

        protected override void OnStop()
        {
            if (null != m_appServer && m_appServer.State == ServerState.Running)
            {
                m_appServer.Stop();
            }
        }
    }

    public class DebuggerModel
    {
        public string Operation { get; set; }
        public Breakpoint Breakpoint { get; set; }
    }
}
