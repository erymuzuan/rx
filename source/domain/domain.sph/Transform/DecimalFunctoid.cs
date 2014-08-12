using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Parse decimal", BootstrapIcon = "usd")]
    public partial class DecimalFunctoid : Functoid
    {
        public const string DEFAULT_STYLES = "None";
        public override sealed bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true, Default = DEFAULT_STYLES });
            return base.Initialize();
        }
        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();

            if (null == this["value"])
                errors.Add("value", "You'll need source for value");

            if (string.IsNullOrWhiteSpace(this.Styles) && null == this["styles"])
                errors.Add("styles", "You'll need either NumberStyles string or source for styles");
            if (!string.IsNullOrWhiteSpace(this.Styles) && null != this["styles"])
                errors.Add("styles", "You'll need either NumberStyles string or source for styles, not both");

            return errors;
        }

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLinf("var val{0} = {1};", this.Index, this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());

          
            var style = this["styles"].GetFunctoid(this.TransformDefinition);
            if (null == style)
            {
                code.AppendFormat("var styles{0} = System.Globalization.NumberStyles.{1};", this.Index, this.Styles ?? DEFAULT_STYLES);
            }
            else
            {
                code.AppendLine(style.GenerateStatementCode());
                code.AppendFormat("var styles{0} = {1};", this.Index, style.GenerateAssignmentCode());
            }
            code.AppendLine();


            return code.ToString();
        }
        public override string GenerateAssignmentCode()
        {
            return string.Format("decimal.Parse(val{0}, styles{0})", this.Index);
        }
    }
}