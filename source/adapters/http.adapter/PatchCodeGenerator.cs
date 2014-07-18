using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PatchCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var content = new StringContent(\"TODO\");");
            code.AppendLine("var response = await client.PatchAsync(url, content);");
            return code.ToString();
        }
    }
}