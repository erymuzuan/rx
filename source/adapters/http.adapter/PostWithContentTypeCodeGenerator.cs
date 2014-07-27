using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostWithContentTypeCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var requestMessage = new  HttpRequestMessage(HttpMethod.Post,url);");
            code.AppendLine("requestMessage.Content = new StringContent(request.PostData, Encoding.UTF8);");
            code.AppendLine("requestMessage.Content.Headers.Remove(\"Content-Type\");");
            code.AppendLinf("requestMessage.Content.Headers.TryAddWithoutValidation(\"Content-Type\", \"{0}\");", operation.GetRequestHeader("Content-Type"));

            return code.ToString();
        }
    }
}