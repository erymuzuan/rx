using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class JsonRequestHarProcess : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            var mimeType = jt.SelectToken("request.postData.mimeType");
            if (null != mimeType && mimeType.Value<string>() == "application/json")
                return true;
            return false;
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {  

            var json = jt.SelectToken("request.postData.text").Value<string>();
            var formFields = JsonSchemaHelper.GenerateSchema(json);
            op.RequestMemberCollection.AddRange(formFields);


        }
    }
}