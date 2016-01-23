using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Helpers
{
    public class JsonContent : StringContent
    {
        public JsonContent(string content) : base(content, Encoding.UTF8, "application/json")
        {
        }

        public JsonContent(string content, Encoding encoding) : base(content, encoding, "application/json")
        {
        }

        public JsonContent(string content, Encoding encoding, string mediaType) : base(content, encoding, mediaType)
        {
        }


    }

    public class JsonResponseMessage : HttpResponseMessage
    {
        public JsonResponseMessage( HttpStatusCode code,object obj) : base(code)
        {
            var json = JsonConvert.SerializeObject(obj);
            this.Content = new JsonContent(json);
        }
        public JsonResponseMessage(object obj) : base(HttpStatusCode.OK)
        {
            var json = JsonConvert.SerializeObject(obj);
            this.Content = new JsonContent(json);
        }
        public JsonResponseMessage(string json) : base(HttpStatusCode.OK)
        {
            this.Content = new JsonContent(json);
        }
    }
}