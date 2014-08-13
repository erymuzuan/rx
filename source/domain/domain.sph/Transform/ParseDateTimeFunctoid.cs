using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Date Parsing", BootstrapIcon = "calendar")]
    public partial class ParseDateTimeFunctoid : Functoid
    {
        public const string DEFAULT_FORMAT = "yyyy-MM-dd";
        public const string DEFAULT_STYLES = "None";
        public const string DEFAULT_CULTURE = "System.Globalization.CultureInfo.InvariantCulture";

        public override sealed bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "format", Type = typeof(string), IsOptional = true, Default = DEFAULT_FORMAT });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true, Default = DEFAULT_STYLES });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "culture", Type = typeof(string), IsOptional = true, Default = DEFAULT_STYLES });
            return base.Initialize();
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = (await base.ValidateAsync()).ToList();

            if (string.IsNullOrWhiteSpace(this.Format) && null == this["format"])
                errors.Add("format", "You'll need either Format string or source for formatting");
            if (!string.IsNullOrWhiteSpace(this.Format) && null != this["format"])
                errors.Add("format", "You'll need either Format string or source for formatting, not both");

            if (string.IsNullOrWhiteSpace(this.Styles) && null == this["styles"])
                errors.Add("styles", "You'll need either DateTimeStyles string or source for styles");
            if (!string.IsNullOrWhiteSpace(this.Styles) && null != this["styles"])
                errors.Add("styles", "You'll need either DateTimeStyles string or source for styles, not both");


            return errors;
        }

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLinf("var value{0} = {1};", this.Index, this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());

            var format = this["format"].GetFunctoid(this.TransformDefinition);
            if (null == format)
                code.AppendLinf("var format{0} = \"{1}\";", this.Index, this.Format ?? DEFAULT_FORMAT);
            else
                code.AppendLinf("var format{0} = {1};", this.Index, format.GenerateAssignmentCode());

            var culture = this["culture"].GetFunctoid(this.TransformDefinition);
            if (null == culture)
            {
                if (string.IsNullOrWhiteSpace(this.Culture))
                    code.AppendLinf("var culture{0} = {1};", this.Index, DEFAULT_CULTURE);
                else
                    code.AppendLinf("var culture{0} = new System.Globalization.CultureInfo(\"{1}\");", this.Index, this.Culture);
            }
            else
                code.AppendLinf("var culture{0} = {1};", this.Index, culture.GenerateAssignmentCode());


            var style = this["styles"].GetFunctoid(this.TransformDefinition);
            if (null == style)
                code.AppendFormat("var style{0} = System.Globalization.DateTimeStyles.{1};", this.Index, this.Styles ?? DEFAULT_STYLES);
            else
                code.AppendFormat("var style{0} = {1};", this.Index, style.GenerateAssignmentCode());

            code.AppendLine();


            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("DateTime.ParseExact(value{0}, format{0}, culture{0}, style{0})", this.Index);
        }
        public override string GetEditorView()
        {
            return Properties.Resources.ParseDateTimeFunctoidHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.ParseDateTimeFunctoidJs;
        }
    }
}