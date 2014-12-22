using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Parallel", TypeName = "Parallel", Description = "Run seperate activities concurently")]
    public partial class ParallelActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.ParallelBranchCollection.Count < 2)
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ParalllelActivity] -> {0} must contains at least 2 branches", this.Name)));
            var errors = from a in this.ParallelBranchCollection
                         where string.IsNullOrWhiteSpace(a.NextActivityWebId)
                         select new BuildError(this.WebId)
                         {
                             Message = string.Format("[ParallelActivity] -> Branch \"{0}\" is missing next activity", a.Name)
                         };
            result.Errors.AddRange(errors);
            return result;
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            var code = new StringBuilder();

            
            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       this.State = \"Ready\";");
            var branches = this.ParallelBranchCollection.Select(b => "\"" + b.NextActivityWebId + "\"");
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{{0}}};", string.Join(",", branches));

            code.AppendLine(this.ExecutedCode);
            
            return code.ToString();
        }
    }
}