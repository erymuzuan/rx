using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Split", FontAwesomeIcon = "columns", Category = "string")]
    public class SplitFunctoid : Functoid
    {
        public const string DEFAULT_STYLES = "None";

        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "delimiter", Type = typeof(string),IsOptional = true, Default = ","});
            this.ArgumentCollection.Add(new FunctoidArg { Name = "type", Type = typeof(string), IsOptional = true, Default = "System.String, mscorlib" });
            return true;
        }


        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            var assignment = this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode();
            code.AppendLine($"var temp{this.Index} = {assignment};");

            var type = this["type"].GetFunctoid(this.TransformDefinition);
            code.AppendLine(null == type
                ? $"var type{Index} = typeof(string);"
                : $"var type{this.Index} = {type.GenerateAssignmentCode()};");

            var delimiter = this["delimiter"].GetFunctoid(this.TransformDefinition)?.GenerateAssignmentCode() ?? ",";
            code.AppendLine($@"var val{Index} = temp{Index}.Split(new string[]{{ ""{delimiter}""}}, StringSplitOptions.RemoveEmptyEntries);");


            return code.ToString();
        }
        // asignment is =, but could AddRange or push
        public override string GenerateAssignmentCode()
        {
            return $"val{Index}";
        }
    }
}