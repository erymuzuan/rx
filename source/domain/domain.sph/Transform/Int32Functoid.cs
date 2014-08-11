using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Parse int", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class Int32Functoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "source", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true, Default = "None" });
            return true;
        }

        public override string GenerateStatementCode()
        {
            this.NumberStyles = "None";
            var code = new StringBuilder();
            code.AppendLinf("               var val{0} = {1};", this.Index,
                this["source"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());
            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("int.Parse(val{0}, System.Globalization.NumberStyles.{1})", this.Index, this.NumberStyles);
        }
    }
}