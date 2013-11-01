using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();

            code.AppendFormat("public partial class Workflow{0}Controller : Controller", wd.WorkflowDefinitionId);
            code.AppendLine("   {");
            code.AppendLine("       public ActionResult "+ this.Title +"()");
            code.AppendLine("       {");

            code.AppendLine("              var html = new System.Text.StringBuilder();");
            foreach (var formElement in this.FormDesign.FormElementCollection)
            {
                code.AppendLine("               html.AppendLine(\"" + formElement.GenerateMarkup() + "\");");
            }
            code.AppendLine("               return html.ToString();");
            code.AppendLine("       }");

            code.AppendLine("   }");


            return code.ToString();
        }
    }
}