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
            var identifier = this.IsStatic ? this.TypeName : "k";
            var code = new StringBuilder();
            code.AppendLinf("   var ca = @\"{0}\".DeserializeFromJson<AssemblyAction>();", this.ToJsonString(true).Replace("\"", "\"\""));
            if (!this.IsStatic)
                code.AppendLine($"   var k = new {this.TypeName}();");
            code.AppendLine("   var context = new RuleContext(item);");
            code.AppendLine();
            var args = string.Join(",", this.MethodArgCollection.Select((x, i) => x.Name + i));
            var count = 0;
            foreach (var arg in this.MethodArgCollection)
            {
                code.AppendLine($"   var {arg.Name}Arg = ca.MethodArgCollection.Single(x => x.Name == \"{arg.Name}\");");
                code.AppendLine($"   var {arg.Name}{count} = ({arg.Type.ToCSharp()}){arg.Name}Arg.GetValue(context);");
                code.AppendLine();
                count++;
            }

            if (this.IsAsyncMethod)
            {
                // TODO : if the async method return Task instead of Task<T> then
                if (this.ReturnType == "System.Threading.Tasks.Task")
                {
                    code.AppendLine($"  await {identifier}.{this.Method}({args});");
                    code.AppendLine("   return 0;");
                    return code.ToString();
                }

                if (!string.IsNullOrWhiteSpace(this.ReturnType) && this.ReturnType.Contains("System.Threading.Tasks.Task`"))
                {
                    code.AppendLine($"  var response = await {identifier}.{this.Method}({args});");
                    code.AppendLine("   return response;");
                    return code.ToString();
                }
            }
            else
            {
                if (this.IsVoid)
                {
                    code.AppendLine($"   {identifier}.{this.Method}({args});");
                    code.AppendLine("   return 0;");
                    return code.ToString();

                }
                code.AppendLine($"   var response = {identifier}.{this.Method}({args});");
                code.AppendLine("   return response");
            }
            return code.ToString();
        }

    }
}