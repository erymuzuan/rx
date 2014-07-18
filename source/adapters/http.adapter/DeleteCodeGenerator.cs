namespace Bespoke.Sph.Integrations.Adapters
{
    public class DeleteCodeGenerator : HttpClientSendCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            return "var response = await client.DeleteAsync(url);";
        }
    }
}