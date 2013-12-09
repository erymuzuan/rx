using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class ListenActivity : Activity
    {
        public override bool IsAsync
        {
            get { return true; }
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            var errors = from a in this.ListenBranchCollection
                         where string.IsNullOrWhiteSpace(a.NextActivityWebId)
                         select new BuildError(this.WebId)
                         {
                             Message = string.Format("[ListenActivity] -> Branch \"{0}\" is missing next activity", a.Name)
                         };
            var errors2 = from a in this.ListenBranchCollection
                          where !string.IsNullOrWhiteSpace(a.NextActivityWebId)
                          let next = wd.ActivityCollection.SingleOrDefault(t => t.WebId == a.NextActivityWebId)
                          where !next.IsAsync
                          select new BuildError(this.WebId)
                          {
                              Message = string.Format("[ListenActivity] -> Branch \"{0}\" is not an async activity", a.Name)
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
                t.AfterExcuteCode = string.Format("     await this.FireListenTrigger{1}(\"{0}\");", t.WebId, this.MethodName);
            }
        }

        public override string GeneratedInitiateAsyncCode(WorkflowDefinition wd)
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
            code.AppendLine("       return new InitiateActivityResult{Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("   }");

            return code.ToString();
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();

            // triggered fires
            code.AppendLinf("   public async Task FireListenTrigger{0}(string webId)", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("       var self = this.GetActivity<ListenActivity>(\"{0}\");", this.WebId);
            code.AppendLinf("       var fired = this.GetActivity<Activity>(webId);");
            code.AppendLinf(@"       
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



            code.AppendLinf("   public Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       throw new System.Exception(\"Listen activity is not supposed to be executed directly but through FireListenTrigger\");");
            code.AppendLine("   }");


            return code.ToString();
        }




    }
}