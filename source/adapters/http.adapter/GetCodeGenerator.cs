using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class GetCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            if (string.IsNullOrWhiteSpace(operation.GetRequestRoute))
                return "var response = await client.GetAsync(url);";

            var code = new StringBuilder();
            code.AppendLine("var curl = request.GenerateUrl(url);");
            code.AppendLine("var response = await client.GetAsync(curl);");
            return code.ToString();

        }
    }
}