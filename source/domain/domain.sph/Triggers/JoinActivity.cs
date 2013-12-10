using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
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


            return code.ToString();
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();

            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       await Task.Delay(40);");
            code.AppendLine("       this.State = \"Ready\";");
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);

            code.AppendLine(this.ExecutedCode);
            code.AppendLine("       return result;");
            code.AppendLine("   }");
            return code.ToString();
        }

        public override void BeforeGenerate(WorkflowDefinition wd)
        {
            // TODO : insert Executed code for each predecessors
            // TODO : InitiateAsync once the first one is fired
        }
    }
}