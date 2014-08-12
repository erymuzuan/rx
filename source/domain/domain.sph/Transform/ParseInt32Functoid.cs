using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Parse int", FontAwesomeIcon = "sort-numeric-asc")]
    public partial class ParseInt32Functoid : Functoid
    {
        public const string DEFAULT_STYLES = "None";

        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true, Default = "None" });
            return true;
        }


        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLinf("var val{0} = {1};", this.Index, this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());


            var style = this["styles"].GetFunctoid(this.TransformDefinition);
            if (null == style)
                code.AppendFormat("var styles{0} = System.Globalization.NumberStyles.{1};", this.Index, this.Styles ?? DEFAULT_STYLES);
            else
                code.AppendFormat("var styles{0} = {1};", this.Index, style.GenerateAssignmentCode());
            
            code.AppendLine();


            return code.ToString();
        }
        public override string GenerateAssignmentCode()
        {
            return string.Format("int.Parse(val{0}, styles{0})", this.Index);
        }
    }
}