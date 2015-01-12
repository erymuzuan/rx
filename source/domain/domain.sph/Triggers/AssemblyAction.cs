using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
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
            get { return this.IsAsyncMethod; }
        }

        public override bool UseCode
        {
            get { return true; }
        }

        public override string GeneratorCode()
        {
            var code = new StringBuilder();
            code.AppendLinf("   var k = new {0}();", this.TypeName);
            code.AppendLinf("   var ca = @\"{0}\".DeserializeFromJson<AssemblyAction>();", this.ToJsonString(true).Replace("\"", "\"\""));
            code.AppendLine("   var context = new RuleContext(item);");
            code.AppendLine();
            var args = string.Join(",", this.MethodArgCollection.Select((x,i) => x.Name + i));
            var count = 0;
            foreach (var arg in this.MethodArgCollection)
            {
                code.AppendLinf("   var field{0} = ca.MethodArgCollection.Single(x =>x.Name == \"{0}\");", arg.Name);
                code.AppendLinf("   var {0}{2} = ({1})field{0}.GetValue(context);", arg.Name, arg.Type.FullName, count++);
                code.AppendLine();
            }
            code.AppendLinf(
                this.IsAsyncMethod ? "   var response = await k.{0}({1});" : "   var response = k.{0}({1});",
                this.Method, args);

            code.AppendLine("return response;");
            return code.ToString();
        }

    }
}