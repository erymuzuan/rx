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
                code.AppendLinf("       var initiateTask{0} = this.InitiateAsync{1}();", count, act.MethodName);
                initiateTasks.Add("initiateTask" + count);
                count++;
            }
            code.AppendLinf("       await Task.WhenAll({0});", string.Join(",", initiateTasks));
            code.AppendLinf("       var tracker = await this.GetTrackerAsync();");
            count = 1;
            foreach (var branch in this.ListenBranchCollection)
            {
                code.AppendLinf("       var act{0} = this.GetActivity<Activity>(\"{1}\");", count, branch.NextActivityWebId);
                code.AppendLinf("       var bc{0} = await  initiateTask{0};", count);
                code.AppendLinf("       tracker.AddInitiateActivity(act{0}, bc{0}, System.DateTime.Now.AddSeconds(1));", count);
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
            code.AppendLinf("   public async Task FireListenTrigger{0}(string webId)", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var self = this.GetActivity<ListenActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("       var fired = this.GetActivity<Activity>(webId);");

            code.AppendLinf("       var tracker = await this.GetTrackerAsync();");
            code.AppendLinf("       tracker.AddExecutedActivity(fired);");

            code.AppendLinf(@"      await self.CancelAsync(this);
 
                                    var cancelled = self.ListenBranchCollection
                                                    .Where(a =>a.NextActivityWebId != webId)
                                                    .Select(a => this.GetActivity<Activity>(a.NextActivityWebId))
                                                    .Select(act => act.CancelAsync(this));");
            code.AppendLinf("       await Task.WhenAll(cancelled);");

            // then execute this fired
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       result.NextActivities = new[]{fired.NextActivityWebId};");
            code.AppendLinf("       await this.SaveAsync(\"{0}\", result);", this.WebId);

            code.AppendLine("   }");

            return code.ToString();
        }

        public override async Task CancelAsync(Workflow wf)
        {
            var tracker = await wf.GetTrackerAsync();

            tracker.CancelAsyncList(this.WebId);
            await tracker.SaveAsync();
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            return @"throw new System.Exception(""Listen activity is not supposed to be executed directly but through FireListenTrigger"");";
        }




    }
}