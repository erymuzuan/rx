using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScreenActivity : Activity
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            var code = new StringBuilder();
            foreach (var formElement in this.FormDesign.FormElementCollection)
            {
                code.AppendLine(formElement.GenerateMarkup());
            }
            return code.ToString();
        }
    }
}