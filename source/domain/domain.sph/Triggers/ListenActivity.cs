using System.Text;
using System.Linq;

namespace Bespoke.Sph.Domain
{
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
            const string r = "\r\n";
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
            // set it to -> waiting for one of branch to fire
            code.AppendLine("       this.State = \"WaitingAsync\";");
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");

            code.AppendLine("   }");

            // creates method for each trigger
            // TODO: we should inject a piece of code in these branch to invoke wf on this branch and kill other branches
            var triggers = from a in this.ListenBranchCollection.Select(lb => lb.Trigger)
                           let mc = a.GeneratedExecutionMethodCode(wd)
                           let otherBranchesActivities = this.ListenBranchCollection.Where(b => b.Trigger.WebId != a.WebId).SelectMany(b =>b.ActivityCollection).Select(b => "\"" + b.WebId + "\"")
                           let stub = string.Format("   public Task<ActivityExecutionResult> {0}()" + r, a.MethodName) +
                           "    {" + r +
                           string.Format("      this.ValidExecutableSteps.Add(\"{0}\");" + r, a.NextActivityWebId) +
                           string.Format("      this.ValidExecutableSteps.Remove(\"{0}\");" + r, this.WebId) +
                           string.Format("      this.ForbiddenSteps.AddRange(new []{{{0}}});" + r, string.Join(",",otherBranchesActivities)) +
                           string.Format("      return {0}_Trigger();" + r, a.MethodName) +
                           "    }" + r
                           select stub + mc.Replace(a.MethodName, a.MethodName + "_Trigger");
            triggers.ToList().ForEach(m => code.AppendLine(m));

            // creates method for each branch
            var ms = from a in this.ListenBranchCollection.SelectMany(lb => lb.ActivityCollection)
                     select a.GeneratedExecutionMethodCode(wd);
            ms.ToList().ForEach(m => code.AppendLine(m));

            return code.ToString();
        }


        public override string GeneratedCustomTypeCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            var triggers = from t in this.ListenBranchCollection
                           select t.Trigger.GeneratedCustomTypeCode(wd);

            triggers.ToList().ForEach(m => code.AppendLine(m));



            var ms = from a in this.ListenBranchCollection.SelectMany(lb => lb.ActivityCollection)
                     select a.GeneratedCustomTypeCode(wd);
            ms.ToList().ForEach(m => code.AppendLine(m));


            return code.ToString();
        }

    }
}