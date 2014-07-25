using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var requestMessage = new  HttpRequestMessage(HttpMethod.Delete,url);");
            code.AppendLine("requestMessage.Content = new StringContent(request.PostData);");
            return code.ToString();
        }
    }
}