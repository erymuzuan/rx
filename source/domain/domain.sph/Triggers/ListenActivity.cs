using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Listen", TypeName = "Listen", Description = "Creates a race condition, first one wins")]
    public partial class ListenActivity : Activity
    {
        public override bool IsAsync => true;

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            var errors = from a in this.ListenBranchCollection
                         where string.IsNullOrWhiteSpace(a.NextActivityWebId)
                         select new BuildError(this.WebId)
                         {
                             Message = $"[ListenActivity] -> Branch \"{a.Name}\" is missing next activity"
                         };
            var errors2 = from a in this.ListenBranchCollection
                          where !string.IsNullOrWhiteSpace(a.NextActivityWebId)
                          let next = wd.ActivityCollection.SingleOrDefault(t => t.WebId == a.NextActivityWebId)
                          where !next.IsAsync
                          select new BuildError(this.WebId)
                          {
                              Message = $"[ListenActivity] -> Branch \"{a.Name}\" is not an async activity"
                          };

            result.Errors.AddRange(errors);
            result.Errors.AddRange(errors2);

            return result;
        }


        public override void BeforeGenerate(WorkflowDefinition wd)
        {
            var triggersId = this.ListenBranchCollection.Select(a => a.NextActivityWebId).ToArray();
            var triggers = wd.ActivityCollection.Where(a => triggersId.Contains(a.WebId));
            foreach (var t in triggers)
            {
                t.ExecutedCode = string.Format("     await this.FireListenTrigger{1}(\"{0}\");", t.WebId, this.MethodName);
            }
        }

        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            var count = 1;
            // triggered fires
            code.AppendLinf("   public async Task<InitiateActivityResult> InitiateAsync{0}()", this.MethodName);
            code.AppendLine("   {");
            // call initiate for all the branches for both
            var initiateTasks = new List<string>();
            foreach (var branch in this.ListenBranchCollection)
            {
                var act = wd.GetActivity<Activity>(branch.NextActivityWebId);
                code.AppendLine($"       var initiateTask{count} = this.InitiateAsync{act.MethodName}();");
                initiateTasks.Add("initiateTask" + count);
                count++;
            }
            code.AppendLine($"       await Task.WhenAll({string.Join(",", initiateTasks)});");
            code.AppendLine();
            code.AppendLinf("       var tracker = await this.GetTrackerAsync();");
            count = 1;
            foreach (var branch in this.ListenBranchCollection)
            {
                code.AppendLine($@"       var act{count} = this.GetActivity<Activity>(""{branch.NextActivityWebId}"");");
                code.AppendLine($"       var bc{count} = await  initiateTask{count};");
                code.AppendLine($"       tracker.AddInitiateActivity(act{count}, bc{count}, System.DateTime.Now.AddSeconds(1));");
                code.AppendLine();
                count++;
            }

            code.AppendLine("       var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("       using(var session = context.OpenSession())");
            code.AppendLine("       {");
            code.AppendLine("           session.Attach(tracker);");
            code.AppendLine("           await session.SubmitChanges(\"ListenActivityChildInitiation\");");
            code.AppendLine("       }");
            code.AppendLine("       return new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("   }");


            // triggered fires
            code.AppendLine($"   private async Task FireListenTrigger{MethodName}(string winningActivityWebId)");
            code.AppendLine("   {");
            code.AppendLine($@"       var self = this.GetActivity<ListenActivity>(""{WebId}"");");
            code.AppendLine("       var fired = this.GetActivity<Activity>(winningActivityWebId);");

            code.AppendLine("       var tracker = await this.GetTrackerAsync();");
            code.AppendLine("       tracker.AddExecutedActivity(fired);");

            code.AppendLinf(@"      await self.CancelAsync(this);
 
                                    var cancelled = self.ListenBranchCollection
                                                    .Where(a =>a.NextActivityWebId != winningActivityWebId)
                                                    .Select(a => this.GetActivity<Activity>(a.NextActivityWebId))
                                                    .Select(act => act.CancelAsync(this));");
            code.AppendLinf("       await Task.WhenAll(cancelled);");


            code.AppendLine("   }");

            return code.ToString();
        }

        public override async Task CancelAsync(Workflow wf)
        {
            var tracker = await wf.GetTrackerAsync();
            tracker.CancelAsyncList(this.WebId, cancelStatus: false);
            await tracker.SaveAsync();
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            return @"throw new System.Exception(""Listen activity is not supposed to be executed directly but through FireListenTrigger"");";
        }




    }
}