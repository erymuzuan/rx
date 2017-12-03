using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.WebApi
{
    public class FromJsonPropertyAttribute : ParameterBindingAttribute
    {
        public string Path { get; }

        public FromJsonPropertyAttribute()
        {

        }

        public FromJsonPropertyAttribute(string path)
        {
            Path = path;
        }
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new JsonPropertyParameterBinding(parameter, Path);
        }


        public class JsonPropertyParameterBinding : HttpParameterBinding
        {
            private readonly string m_path;
            public JsonPropertyParameterBinding(HttpParameterDescriptor parameter, string path)
                : base(parameter)
            {
                m_path = path;
            }

            public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
                HttpActionContext actionContext, CancellationToken cancellationToken)
            {
                var request = actionContext.Request;
                var ctx = actionContext.Request.GetOwinContext();
                var stream = request.Content.ReadAsStreamAsync().Result;
                var text = GetContentJson(stream, ctx);
                var setting = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                var json = JObject.Parse(text);

                var path = m_path ?? "$." + Descriptor.ParameterName;
                var token = json.SelectToken(path);
                // TODO, when it's like this { $type:"something", $values :[]}}
                if (token is JArray)
                {
                    var list = JsonConvert.DeserializeObject(token.ToString(), Descriptor.ParameterType, setting);
                    actionContext.ActionArguments[Descriptor.ParameterName] = list;
                }
                else
                {
                    actionContext.ActionArguments[Descriptor.ParameterName] = JsonConvert.DeserializeObject(token.ToString(), setting);
                }

                var tsc = new TaskCompletionSource<object>();
                tsc.SetResult(null);
                return tsc.Task;
            }


            private static string GetContentJson(Stream stream, IOwinContext ctx)
            {
                // We can't read request stream twice, so attach it to OWIN.context
                if (!stream.CanRead) return ctx.Get<string>("RawText");
                if (stream.CanSeek)
                    stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var text = reader.ReadToEnd();
                    ctx.Set("RawText", text);
                    return text;
                }
            }
        }
    }
}