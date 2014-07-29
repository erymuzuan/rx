using System.ComponentModel.Composition;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class RequestPostDataHarProcess : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            return true;
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {  // post data
            var postData = from p in jt.SelectTokens("request.postData.params").SelectMany(x => x)
                let field = p.SelectToken("name")
                where null != field
                      && !string.IsNullOrWhiteSpace(field.Value<string>())
                select new RegexMember
                {
                    FieldName = field.Value<string>(),
                    Type = typeof(string)
                };
            op.RequestMemberCollection.AddRange(postData);
        }
    }
}