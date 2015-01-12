using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class DeleteCodeGenerator : SendCode
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var requestMessage = new  HttpRequestMessage(HttpMethod.Delete,url);");

            return code.ToString();
        }
    }
}