using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataForPostUrlEncoded : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var fields = operation.RequestMemberCollection.OfType<RegexMember>()
                .Where(x => !string.IsNullOrWhiteSpace(x.FieldName));
            var names = string.Join(" + \"&", fields.Select(x => x.FieldName.EscapeDataString() + "=\" + " + x.Name + ".EscapeDataString()"));
            return "               return \"" + names + ";";
        }
    }
}