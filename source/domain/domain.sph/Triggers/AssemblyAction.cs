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
        public override bool UseAsync => this.IsAsyncMethod;

        public override bool UseCode => true;

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
                count++;
                code.AppendLine($"   var field{arg.Name} = ca.MethodArgCollection.Single(x =>x.Name == \"{arg.Name}\");");
                code.AppendLine($"   var {arg.Name}{count} = ({arg.Type.FullName})field{arg.Name}.GetValue(context);");
                code.AppendLine();
            }
            code.AppendLine(
                this.IsAsyncMethod ? $"   var response = await k.{this.Method}({args});" : $"   var response = k.{this.Method}({args});");

            // TODO : if the async method return Task instead of Task<T> then

            code.AppendLine("return response;");
            return code.ToString();
        }

    }
}