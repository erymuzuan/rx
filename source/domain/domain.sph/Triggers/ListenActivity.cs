using System.Text;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class ListenBranch : Activity
    {

    }
    public partial class ListenActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            var errors = from a in this.ListenBranchCollection
                         where null == a.Trigger || !a.Trigger.IsAsync
                         select new BuildError
                         {
                             Message = string.Format("[ListenActivity] -> Branch \"{0}\" is not an async activity", a.Name)
                         };

            result.Errors.AddRange(errors);

            return result;
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {


            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       var script = ObjectBuilder.GetObject<IScriptEngine>();");
            var count = 1;
            code.AppendLinf("       var listen = this.WorkflowDefinition.ActivityCollection.Single(w =>w.WebId ==\"{0}\") as ListenActivity;", this.WebId);
            code.AppendLine();
            // call initiate async for both
            foreach (var branch in this.ListenBranchCollection)
            {
                code.AppendLinf("       var branch{0} = listen.ListenBranchCollection.Single(a => a.WebId == \"{1}\");", count, branch.WebId);
                code.AppendLinf("       var trigger{0} =  branch{0}.Trigger;", count);
                code.AppendLinf("       await trigger{0}.InitiateAsync(this);", count, branch.WebId);
                code.AppendLine();
                count++;
            }
            // set it to still waiting for one of branch to fire
            code.AppendLine("       this.State = \"WaitingAsync\";");
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");

            code.AppendLine("   }");

            // creates method for each branch
            count = 1;
            foreach (var branch in this.ListenBranchCollection)
            {
                //code.AppendLinf("   private bool {0}()", this.GetBranchMethodName(branch));
                //code.AppendLine("   {");
                //code.AppendLine("       var item = this;");
                //code.AppendLinf("       return {0};", branch.Expression);
                //code.AppendLine("   }");
                count++;
            }

            return code.ToString();
        }
    }
}