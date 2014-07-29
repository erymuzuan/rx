using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PutCodeGenerator : SendCode
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var content = new StringContent(\"TODO\");");
            code.AppendLine("var response = await client.PutAsync(url, content);");
            return code.ToString();
        }
    }
}