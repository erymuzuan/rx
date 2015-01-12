namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataForGet : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            return "                return string.Empty;";
        }
    }
}