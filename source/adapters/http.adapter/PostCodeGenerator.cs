using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var content = new StringContent(request.PostData);");
            code.AppendLine("var response = await client.PostAsync(url, content);");
            return code.ToString();
        }
    }
}