using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Loop", FontAwesomeIcon = "refresh")]
    public class LoopingFunctoid : Functoid
    {
        public const string DEFAULT_STYLES = "None";

        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "sourceCollection", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "destinationCollection", Type = typeof(string), IsOptional = true, Default = "None" });
            return true;
        }


        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();

            var source = this["sourceCollection"].GetFunctoid(this.TransformDefinition);
            var dest = this["destinationCollection"];
            code.AppendLinf("var val{0} = from r in {1}", this.Index, source.GenerateAssignmentCode());
            code.AppendLinf("               select new {0} {{", dest.Type.FullName);
            code.AppendLine("};");


            
            code.AppendLinf("dest.{1}.AddRange(val{0});", this.Index, source.GenerateAssignmentCode().Replace("item.",""));


            return code.ToString();
        }
        public override string GenerateAssignmentCode()
        {
            return string.Empty;
        }
    }
}