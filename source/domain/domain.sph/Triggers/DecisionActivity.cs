using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DecisionActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (this.DecisionBranchCollection.Count(b => b.IsDefault) != 1)
                throw new InvalidOperationException("You should have one default branch in \"" + this.Name + "\"");

            var missingNext =
                this.DecisionBranchCollection.Where(b => string.IsNullOrWhiteSpace(b.NextActivityWebId))
                    .Select(b => b.Name).ToArray();
            if (missingNext.Any())
                throw new InvalidOperationException("Thi(ese) branches do not have next activity " + string.Join(",", missingNext));

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       var script = ObjectBuilder.GetObject<IScriptEngine>();");
            var count = 1;
            foreach (var branch in this.DecisionBranchCollection.Where(b => !b.IsDefault))
            {
                code.AppendLinf("       var branch{0} = this.{1}();", count, this.GetBranchMethodName(branch));
                code.AppendLinf("       if(branch{0})", count);
                code.AppendLine("       {");
                code.AppendLinf("           this.CurrentActivityWebId = \"{0}\";", branch.NextActivityWebId);
                code.AppendLinf("           await this.SaveAsync(\"{0}\");", this.WebId);
                code.AppendLine("           return result;");
                code.AppendLine("       }");
                count++;
            }

            // default
            var default1 = this.DecisionBranchCollection.Single(b => b.IsDefault);

            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", default1.NextActivityWebId);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");
            code.AppendLine("   }");// end metod

            // decision branch
            count = 1;
            foreach (var branch in this.DecisionBranchCollection.Where(b => !b.IsDefault))
            {
                code.AppendLinf("   private bool {0}()", this.GetBranchMethodName(branch));
                code.AppendLine("   {");
                code.AppendLine("       var item = this;");
                code.AppendLinf("       return {0};", branch.Expression);
                code.AppendLine("   }");
                count++;
            }

            return code.ToString();
        }

        private string GetBranchMethodName(DecisionBranch branch)
        {
            return this.Name.Replace(" ", string.Empty).Replace("-", "_") + branch.Name.Replace(" ", string.Empty).Replace("-", "_");
        }

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}