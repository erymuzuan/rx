using System.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataForPostUrlEncoded : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var names = string.Join(" + \"&", operation.RequestMemberCollection.Select(x => x.Name + "=\" + " + x.Name));
            return "               return \"" + names + ";";
        }
    }
}