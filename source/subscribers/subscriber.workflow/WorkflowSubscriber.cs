using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            if (item.State == "Completed") return;
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", item.WorkflowDefinitionId, item.Version));
            var tracker = await item.GetTrackerAsync();

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromXml<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;

            var message = new StringBuilder();
            message.AppendLine("Running " + item.Name + ":" + item.WorkflowId);
            // get current activity
            dynamic headers = header;
            string executedActivityWebId = headers.ActivityWebId;
            string nextActivities = headers.NextActivities;
            string[] activities = nextActivities.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var executed = item.GetActivity<Activity>(executedActivityWebId);
            message.AppendLine("Executed : " + executed.Name);

            // debugger
            this.CurrentBreakpoint = this.GetBreakpoint(item, activities);
            if (null != this.CurrentBreakpoint)
            {
                var debugged = item.GetActivity<Activity>(this.CurrentBreakpoint.ActivityWebId);
                this.WriteMessage("DEBUG :" + debugged.Name);
                await this.SendLocalsAsync(item);
                this.CurrentBreakpoint.Break();
                await this.CurrentBreakpoint.WaitAsync();
            }
            var context = new SphDataContext();


            foreach (var id in activities)
            {
                var act = item.GetActivity<Activity>(id);
                if (act.IsAsync)
                {
                    message.AppendLine("Initiating : " + act.Name);
                    var initiatedAsyncMethod = item.GetType().GetMethod("InitiateAsync" + act.MethodName);
                    var task = (Task)initiatedAsyncMethod.Invoke(item, null);
                    task.Wait();

                    tracker.AddInitiateActivity(act);
                    using (var session = context.OpenSession())
                    {
                        session.Attach(tracker);
                        await session.SubmitChanges("Update");
                    }
                }
                else
                {
                    message.AppendLine("Executing : " + act.Name);
                    await item.ExecuteAsync(id);
                }
            }
            this.WriteMessage(message);
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

        private Breakpoint GetBreakpoint(Workflow item, IEnumerable<string> activities)
        {
            return this.BreakpointCollection
                .Where(b => b.WorkflowDefinitionId == item.WorkflowDefinitionId)
                .Where(b => b.IsEnabled)
                .SingleOrDefault(b => activities.Contains(b.ActivityWebId));
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
            if (!m_appServer.Setup(ConfigurationManager.WorkflowDebuggerPort))
            {
                this.WriteError(new Exception("Failed to setup WebSocket Server!"));
                return;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;

            if (!m_appServer.Start())
            {
                this.WriteError(new Exception("Failed to start websocket server!"));
                return;
            }
            this.WriteMessage("The WebSocket server started successfully!");
        }



        private void NewMessageReceived(WebSocketSession session, string value)
        {
            var model = value.DeserializeFromJson<DebuggerModel>();
            if (null == model) return;
            this.WriteMessage("[DEBUG] : " + model.Operation);
            if (null != model.Breakpoint)
            {
                this.WriteMessage("Receieve break point " + model.Breakpoint.ActivityWebId);
                var bp = this.BreakpointCollection.SingleOrDefault(b => b.ActivityWebId == model.Breakpoint.ActivityWebId);
                switch (model.Operation)
                {
                    case "AddBreakpoint":
                        if (null != bp)
                            this.BreakpointCollection.Replace(bp, model.Breakpoint);
                        else
                            this.BreakpointCollection.Add(model.Breakpoint); break;
                    case "RemoveBreakpoint":
                        this.BreakpointCollection.Remove(bp);
                        break;
                }

            }
            if(null == this.CurrentBreakpoint)return;

            switch (model.Operation)
            {
                case "Continue":
                    this.CurrentBreakpoint.Continue();
                    break;
                case "Stop":
                    this.CurrentBreakpoint.Stop();
                    break;
                case "StepIn":
                    this.CurrentBreakpoint.StepIn();
                    break;
                case "StepOut":
                    this.CurrentBreakpoint.StepOut();
                    break;
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
