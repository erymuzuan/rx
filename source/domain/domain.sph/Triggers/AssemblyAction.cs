using System.ComponentModel.Composition;
using System.Drawing;
using System.Text;
using Bespoke.Sph.Domain.Properties;

namespace Bespoke.Sph.Domain
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Custom assembly", TypeName = "Bespoke.Sph.Domain.AssemblyAction, domain.sph", Description = "Execute a method is a custom assembly", FontAwesomeIcon = "gear")]
    public partial class AssemblyAction : CustomAction
    {
        public override string GetEditorView()
        {
            return Resources.action_assembly_html;
        }

        public override string GetEditorViewModel()
        {
            return Resources.action_assembly_js;
        }

        public override Bitmap GetPngIcon()
        {
            return Resources.Gear;
        }
        public override bool UseAsync
        {
            get { return true; }
        }

        public override bool UseCode
        {
            get { return true; }
        }

        public override string GeneratorCode()
        {
            var code = new StringBuilder();


            code.AppendLinf("   var k = new {0}();");
            if(this.IsAsyncMethod)
                code.AppendLinf("   var response = await k.{0}(item);");
            else
                code.AppendLinf("   var response = k.{0}(item);");

            code.AppendLine("return response;");
            return code.ToString();
        }

    }
}