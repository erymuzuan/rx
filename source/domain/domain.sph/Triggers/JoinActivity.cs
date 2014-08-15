using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Join", Description = "Wait for concurrent activities")]
    public partial class JoinActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var predecessors = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId).ToList();
            var result = base.ValidateBuild(wd);
            if (!predecessors.Any())
                result.Errors.Add(new BuildError(this.WebId, string.Format("[JoinActivity] -> {0} does not have any predecessor", this.Name)));
            if (predecessors.Count < 2)
                result.Errors.Add(new BuildError(this.WebId, string.Format("[JoinActivity] -> {0} must have at least 2 predecessors", this.Name)));

            if (predecessors.OfType<EndActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, string.Format("[JoinActivity] -> {0} , EndActivity is invalid predecessor", this.Name)));
            if (predecessors.OfType<DecisionActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, string.Format("[JoinActivity] -> {0} , DecisionActivity is invalid predecessor", this.Name)));
            if (predecessors.OfType<ListenActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, string.Format("[JoinActivity] -> {0} , ListenActivity is invalid predecessor", this.Name)));

            return result;
        }

        public override bool IsAsync
        {
            get { return true; }
        }

        public override string GeneratedInitiateAsyncCode(WorkflowDefinition wd)
        {
            var predecessors = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId);
            var code = new StringBuilder();

            code.AppendLinf("   public async Task FireJoin{0}(string webid)", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var tracker = await this.GetTrackerAsync();");
            code.AppendLinf("       if(!tracker.WaitingJoinList.ContainsKey(\"{0}\"))", this.WebId);
            code.AppendLine("       {");
            foreach (var act in predecessors)
            {
                code.AppendLinf("           tracker.AddJoinWaitingList(\"{0}\",\"{1}\");", this.WebId, act.WebId);
            }
            code.AppendLine("       }");
            code.AppendLinf("       tracker.AddFiredJoin(\"{0}\", webid);", this.WebId);
            code.AppendLine("       await tracker.SaveAsync();");
            code.AppendLine();

            // TODO : we need the correlation too? I guessed
            code.AppendLinf("       if(tracker.AllJoinFired(\"{0}\"))", this.WebId);
            code.AppendLine("       {");
            code.AppendLine("           Console.WriteLine(\"Everthing is ok\");");
            code.AppendLine("           var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("           result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);
            code.AppendLinf("           await this.SaveAsync(\"{0}\", result);", this.WebId);
            code.AppendLine("       }");
            code.AppendLine("   }");

            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       return new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("   }");


            return code.ToString();
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();

            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       throw new System.Exception(\"Listen activity is not supposed to be executed directly but through FireListenTrigger\");");
            code.AppendLine("   }");
            return code.ToString();
        }

        public override void BeforeGenerate(WorkflowDefinition wd)
        {
            // TODO : insert Executed code for each predecessors

            var predecessors = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId);
            foreach (var act in predecessors)
            {
                var code = string.Format("   await this.FireJoin{0}(\"{1}\");", this.MethodName, act.WebId);
                act.ExecutedCode += code;
            }
            // TODO : InitiateAsync once the first one is fired
        }
    }
}