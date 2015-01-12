using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class GetCodeGenerator : SendCode
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var code = new StringBuilder();
            code.AppendLine("var requestMessage = new  HttpRequestMessage(HttpMethod.Get,url);");

            return code.ToString();


        }
    }
}