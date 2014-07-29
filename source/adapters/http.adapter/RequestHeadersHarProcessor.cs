using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Humanizer;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class RequestHeadersHarProcessor : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            return true;
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {
            var requestHeaders = from p in jt.SelectTokens("request.headers").SelectMany(x => x)
                let ov = p.SelectToken("value").Value<string>()
                select new HttpHeaderDefinition
                {
                    Name = p.SelectToken("name").Value<string>(),
                    DefaultValue = ov,
                    OriginalValue = ov,
                    Field = new ConstantField
                    {
                        Value = ov,
                        Type = typeof(string),
                        Name = ov.Truncate(20, "..."),
                        WebId = Guid.NewGuid().ToString()
                    }
                };
            op.RequestHeaderDefinitionCollection.ClearAndAddRange(requestHeaders);
        }
    }
}