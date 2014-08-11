using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Fomatting object", FontAwesomeIcon = "file-text")]
    public partial class FormattingFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(object) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "format", Type = typeof(string), IsOptional = true });

            return base.Initialize();
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errrors =( await base.ValidateAsync()).ToList();
            if(string.IsNullOrWhiteSpace(this.Format) && string.IsNullOrWhiteSpace(this["format"].Functoid))
                errrors.Add("format", "you must supplier either format field or a source", this.WebId);
            if(!string.IsNullOrWhiteSpace(this.Format) && !string.IsNullOrWhiteSpace(this["format"].Functoid))
                errrors.Add("format", "you must supplier either format field or a source not both", this.WebId);
            return errrors;
        }

        private int m_number;
        public override string GenerateStatementCode()
        {
            this.m_number = GetRunningNumber();

            var format = this["format"].GetFunctoid(this.TransformDefinition);
            var value = this["value"].GetFunctoid(this.TransformDefinition);

            var code = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.Format))
                code.AppendLinf("       var format{1} = \"{0}\";", this.Format, m_number);
            else
            {
                code.AppendLine("       " + format.GenerateStatementCode());
                code.AppendLinf("       var format{0} = {1};",m_number, format.GenerateAssignmentCode());
            }

            code.AppendLine("       " + value.GenerateStatementCode());
            code.AppendLinf("       var value{0} = {1}; ",m_number, value.GenerateAssignmentCode());

            code.AppendLinf("   ");
            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            if (!string.IsNullOrWhiteSpace(this.Format))
                return string.Format("string.Format(\"{1}\", value{0})",m_number, this.Format);
            return string.Format("string.Format( format{0}, value{0})",m_number);
        }

        public override string GetEditorView()
        {
            return Properties.Resources.FormattingFunctoidHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.FormattingFunctoidJs;
        }
    }
}