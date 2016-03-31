using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Join", TypeName = "Join", Description = "Wait for concurrent activities")]
    public partial class JoinActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var branches = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId).ToList();
            var result = base.ValidateBuild(wd);
            if (!branches.Any())
                result.Errors.Add(new BuildError(this.WebId, $"[JoinActivity] -> {this.Name} does not have any predecessor"));
            if (branches.Count < 2)
                result.Errors.Add(new BuildError(this.WebId, $"[JoinActivity] -> {this.Name} must have at least 2 parallel branches"));

            if (branches.OfType<EndActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, $"[JoinActivity] -> {this.Name} , EndActivity is invalid branch"));
            if (branches.OfType<DecisionActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, $"[JoinActivity] -> {this.Name} , DecisionActivity is invalid branch"));
            if (branches.OfType<ListenActivity>().Any())
                result.Errors.Add(new BuildError(this.WebId, $"[JoinActivity] -> {this.Name} , ListenActivity is invalid branch"));

            return result;
        }

        public override bool IsAsync => true;

        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var branches = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId);
            var code = new StringBuilder();

            code.AppendLine($"   public async Task FireJoin{MethodName}(string webid)");
            code.AppendLine("   {");
            code.AppendLine("       var tracker = await this.GetTrackerAsync().ConfigureAwait(false);");
            code.AppendLine($"       if(!tracker.WaitingJoinList.ContainsKey(\"{WebId}\"))");
            code.AppendLine("       {");
            foreach (var br in branches)
            {
                code.AppendLine($"           tracker.AddJoinWaitingList(\"{WebId}\",\"{br.WebId}\");");
            }
            code.AppendLine("       }");
            code.AppendLine($"       tracker.AddFiredJoin(\"{WebId}\", webid);");
            code.AppendLine("       await tracker.SaveAsync().ConfigureAwait(false);");
            code.AppendLine();

            // TODO : we need the correlation too? I guessed
            code.AppendLine($"       if(tracker.AllJoinFired(\"{WebId}\"))");
            code.AppendLine("       {");
            code.AppendLine("           Console.WriteLine(\"Everthing is ok\");");
            code.AppendLine("           var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine($"           result.NextActivities = new[]{{\"{NextActivityWebId}\"}};");
            code.AppendLinf($"           await this.SaveAsync(\"{WebId}\", result).ConfigureAwait(false);");
            code.AppendLine("       }");
            code.AppendLine("   }");

            code.AppendLine($"   public Task<InitiateActivityResult> InitiateAsync{MethodName}()");
            code.AppendLine("   {");
            code.AppendLine("       var result = new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("       return Task.FromResult(result);");
            code.AppendLine("   }");


            return code.ToString();
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            return $@"throw new System.Exception(""Join activity is not supposed to be executed directly but through FireJoin{MethodName}"");";
        }

        public override void BeforeGenerate(WorkflowDefinition wd)
        {
            // insert Executed code for each branches
            var branches = wd.ActivityCollection.Where(a => a.NextActivityWebId == this.WebId);
            foreach (var act in branches)
            {
                act.ExecutedCode = $"   await this.FireJoin{this.MethodName}(\"{act.WebId}\").ConfigureAwait(false);"; ;
            }
            // TODO : InitiateAsync once the first one is fired
        }
    }
}