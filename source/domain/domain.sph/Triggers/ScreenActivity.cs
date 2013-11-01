﻿using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();

            code.AppendFormat("public partial class Workflow{0}Controller : System.Web.Mvc.Controller", wd.WorkflowDefinitionId);
            code.AppendLine("   {");
            code.AppendLine("       public System.Web.Mvc.ActionResult "+ this.Title +"()");
            code.AppendLine("       {");

            code.AppendLine("              var html = new System.Text.StringBuilder();");
            foreach (var formElement in this.FormDesign.FormElementCollection)
            {
                code.AppendLine("               html.AppendLine(\"" + formElement.GenerateMarkup() + "\");");
            }
            code.AppendLine("               return Content(html.ToString());");
            code.AppendLine("       }");

            code.AppendLine("   }");


            return code.ToString();
        }
    }
}