using System;
using System.ComponentModel.Composition;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class RequestQueryStringHarProcess : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            return true;
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {
            var qs = jt.SelectTokens("request.queryString");
            if (null != qs && qs.Any())
            {
                var kvps = (from p in jt.SelectTokens("request.queryString").SelectMany(x => x)
                            let ov = p.SelectToken("value").Value<string>()
                            let name = p.SelectToken("name").Value<string>()
                            select string.Format("{0}={{{0}}}", name))
                           .ToArray();
                if (kvps.Length > 0)
                {
                    var kvps2 = from p in jt.SelectTokens("request.queryString").SelectMany(x => x)
                                let ov = p.SelectToken("value").Value<string>()
                                let name = p.SelectToken("name").Value<string>()
                                select new RegexMember { Name = name, Type = typeof(string) };
                    op.RequestMemberCollection.ClearAndAddRange(kvps2);

                    var url = jt.SelectToken("request.url").Value<string>();
                    var uri = new Uri(url);
                    op.Url = uri.AbsolutePath;

                    op.RequestRouting = op.Url + "?" + string.Join("&", kvps.ToArray());
                }


            }
        }
    }
}