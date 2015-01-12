using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class DefaultNameSuggestionHarProcessor : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            return true;
        }
        public void Process(HttpOperationDefinition op, JToken jt)
        {
            var uri = new System.Uri(jt.SelectToken("request.url").Value<string>());
            op.Url = uri.AbsolutePath;

            var name = op.Url.Replace("%20", "_")
                     .Replace("%2C", "_")
                     .Replace(" ", "_")
                     .Replace("/", "_")
                     .Replace(".", "_")
                     .Replace("__", "_")
                     ;
            op.MethodName = name.ToCsharpIdentitfier();
            op.Name = op.MethodName;
        }
    }
}