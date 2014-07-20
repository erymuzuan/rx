namespace Bespoke.Sph.Integrations.Adapters
{
    public class GetCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
           return "var response = await client.GetAsync(url);";


        }
    }
}