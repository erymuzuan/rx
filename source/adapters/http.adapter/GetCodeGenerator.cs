using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class GetCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var requestMessage = new  HttpRequestMessage(HttpMethod.Get,url);");

            return code.ToString();


        }
    }
}