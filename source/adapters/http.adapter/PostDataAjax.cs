using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataAjax : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("           var json = JsonConvert.SerializeObject(this);");
            code.AppendLine("           return json;");
            return code.ToString();
        }
    }
}