using System;
using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{

    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Mapping", TypeName = "Mapping", Description = "Run a data transform")]
    public partial class MappingActivity : Activity
    {

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = new BuildValidationResult { Result = true };
            if (string.IsNullOrWhiteSpace(OutputPath))
                result.Errors.Add(new BuildError(this.WebId, "[MappingActivity] : Please set the output path for the mapping"));
            if (this.MappingSourceCollection.Count == 0)
                result.Errors.Add(new BuildError(this.WebId, "[MappingActivity] : Please set at least one mapping source"));


            result.Result = result.Errors.Count == 0;
            return result;
        }
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);


            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");

            code.AppendLinf("        var map = new {0}();", this.MappingDefinition);
            code.AppendLinf("        this.{0} = await map.TransformAsync({1});", this.OutputPath, this.MappingSourceCollection[0].Variable);
           


            code.AppendLine();
            // set the next activity
            code.AppendLine("       var ear = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       ear.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);/* webid*/
            code.AppendLine("       return ear;");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}