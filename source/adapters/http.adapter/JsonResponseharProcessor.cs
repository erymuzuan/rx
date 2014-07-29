using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    [Export(typeof(IHarProcessor))]
    public class JsonResponseHarProcessor : IHarProcessor
    {
        public bool CanProcess(HttpOperationDefinition op, JToken jt)
        {
            var responseMime = jt.SelectToken("response.content.mimeType");
            if (null != responseMime && responseMime.Value<string>() == "application/json; charset=utf-8")
                return true;
            return false;
        }

        public void Process(HttpOperationDefinition op, JToken jt)
        {

            var json = jt.SelectToken("response.content.text").Value<string>();
            op.ResponseIsJsonArray = json.Trim().StartsWith("[");
            if (op.ResponseIsJsonArray)// for array
                json = "{\"list\":" + json + "}";

            try
            {
                var formFields = JsonSerializerService.GenerateSchema(json);
                op.ResponseMemberCollection.AddRange(formFields);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(json);
                Debugger.Break();
            }


        }
    }
}