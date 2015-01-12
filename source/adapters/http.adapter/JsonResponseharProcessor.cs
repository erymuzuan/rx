using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
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

            var commentNode = jt.SelectToken("response.content.comment");
            var textNode = jt.SelectToken("response.content.text");
            if (null != commentNode && null == textNode)
            {
                var comment = jt.SelectToken("response.content.comment").Value<string>();
                throw new InvalidOperationException("Cannot process the node : " + comment);
            }
            var json = textNode.Value<string>();
            op.ResponseIsJsonArray = json.Trim().StartsWith("[");
            if (op.ResponseIsJsonArray)// for array
                json = "{\"list\":" + json + "}";

            try
            {
                var formFields = JsonSchemaHelper.GenerateSchema(json);
                op.ResponseMemberCollection.ClearAndAddRange(formFields);
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