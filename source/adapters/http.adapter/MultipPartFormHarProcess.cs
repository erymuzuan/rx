using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class MultipPartFormHarProcess : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            var mimeType = jt.SelectToken("request.postData.mimeType");
            return null != mimeType && mimeType.Value<string>() == "multipart/form-data";
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {

            var text = jt.SelectToken("request.postData.text").Value<string>();
            var formFields = from f in Strings.RegexValues(text, @"name=\""(?<fname>.*?)\""", "fname")
                select new RegexMember
                {
                    FieldName = f,
                    Type = typeof(string)
                };
            op.RequestMemberCollection.AddRange(formFields);


        }
    }
}