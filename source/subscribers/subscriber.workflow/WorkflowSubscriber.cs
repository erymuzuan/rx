using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowSubscriber : Subscriber<Workflow>
    {
        public override string QueueName => "workflow_execution";

        public override string[] RoutingKeys => new[] { "Workflow.*.Execute" };


        protected override async Task ProcessMessage(Workflow item, MessageHeaders header)
        {

            if (item.State == "Completed") return;
            var tracker = await item.GetTrackerAsync();
            await item.LoadWorkflowDefinitionAsync();

            var message = new StringBuilder();
            message.AppendLine("Running " + item.Name + ":" + item.Id);
            // get current activity
            dynamic headers = header;
            string executedActivityWebId = headers.ActivityWebId;
            string nextActivities = headers.NextActivities;
            var activities = nextActivities.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var executed = item.GetActivity<Activity>(executedActivityWebId);
            message.AppendLine("Executed : " + executed.Name);

            #region "DEBUGGER"

            // debugger
            this.CurrentBreakpoint = this.GetBreakpoint(item, activities);
            if (null != this.CurrentBreakpoint)
            {
                this.CurrentBreakpoint.Workflow = item;
                this.CurrentBreakpoint.WorkflowDefinition = item.WorkflowDefinition;
                var debugged = item.GetActivity<Activity>(this.CurrentBreakpoint.ActivityWebId);
                this.WriteMessage("DEBUG :" + debugged.Name);
                await this.SendLocalsAsync(item);
                this.CurrentBreakpoint.Break();
                await this.CurrentBreakpoint.WaitAsync();
            }

            #endregion

            var context = new SphDataContext();

            foreach (var id in activities)
            {
                var act = item.GetActivity<Activity>(id);
                if (act.IsAsync)
                {
                    message.AppendLine("Initiating : " + act.Name);
                    var initiatedAsyncMethod = item.GetType().GetMethod("InitiateAsync" + act.MethodName);
                    var task = (Task<InitiateActivityResult>)initiatedAsyncMethod.Invoke(item, null);
                    var initiateResult = await task;

                    tracker.AddInitiateActivity(act, initiateResult);
                    using (var session = context.OpenSession())
                    {
                        session.Attach(tracker);
                        await session.SubmitChanges("Update");
                    }
                }
                else
                {
                    message.AppendLine("Executing : " + act.Name);
                    var result = await item.ExecuteAsync(id);

                    #region "DEBUGGER"

                    if (null != this.CurrentBreakpoint && this.CurrentBreakpoint.Operation == "StepThrough")
                    {
                        var bps = from a in result.NextActivities
                                  select new Breakpoint(item, item.WorkflowDefinition)
                                  {
                                      ActivityWebId = a,
                                      IsEnabled = true,
                                      WorkflowDefinitionId = item.WorkflowDefinitionId
                                  };
                        this.BreakpointCollection.AddRange(bps);
                        Console.WriteLine(@"XXXXXXXXXXXXXX" + this.BreakpointCollection.Count);
                    }

                    #endregion

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
            this.SendMessage(model.ToJsonString());
            return Task.FromResult(0);
        }

        private void SendMessage(string message)
        {
            m_appServer.GetAllSessions().ToList().ForEach(e => e.Send(message));
        }

        private Breakpoint GetBreakpoint(Workflow item, IEnumerable<string> activities)
        {
            return this.BreakpointCollection
                .Where(b => b.WorkflowDefinitionId == item.WorkflowDefinitionId)
                .Where(b => b.IsEnabled)
                .LastOrDefault(b => activities.Contains(b.ActivityWebId));
        }

        public ObjectCollection<Breakpoint> BreakpointCollection { get; } = new ObjectCollection<Breakpoint>();

        private WebSocketServer m_appServer;
        protected override void OnStart()
        {
            var args = Environment.GetCommandLineArgs();
            if (!args.Contains("/debug")) return;


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
                    case "ClearBreakpoint":
                        this.BreakpointCollection.Clear();
                        break;
                }

            }


            if (null == this.CurrentBreakpoint) return;

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
                case "StepThrough":
                    this.CurrentBreakpoint.StepThrough();
                    break;
                case "StepOut":
                    this.CurrentBreakpoint.StepOut();
                    break;
                case "Console":

                    var ret = this.CurrentBreakpoint.EvaluateConsole(model.Console);
                    this.SendMessage(ret.ToJsonString(Formatting.Indented));
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
}
