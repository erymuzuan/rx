using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class PostDataForPostMultipartEncoded : PostDataCodeGenerator
    {
        public override string GenerateCode(HttpOperationDefinition operation)
        {
            var contentType = operation.GetRequestHeader("Content-Type");
            var boundary = Strings.RegexSingleValue(contentType, "boundary=(?<boundary>.*?)$", "boundary");

            var code = new StringBuilder("return string.Format(@\"");
            var count = 0;
            foreach (var member in operation.RequestMemberCollection)
            {
                code.AppendLine("--" + boundary);
                code.AppendFormat("Content-Disposition: form-data; name=\"\"{0}\"\"", member.Name);
                code.AppendLine();
                code.AppendLine();
                code.AppendLinf("{{{0}}}", count++);

            }
            code.AppendLine("--" + boundary + "--\",");

            var names = string.Join(",", operation.RequestMemberCollection.Select(x => x.Name));
            code.AppendLine(names);
            code.AppendLine(");");

            return code.ToString();

        }
    }
}