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

    public class JsonPropertyParameterAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new JsonProperyParameterBinding(parameter);
        }
    }

    public class JsonBodyAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            return new RxEntityParameterBinding(parameter);
        }
    }

    public class RxEntityParameterBinding : HttpParameterBinding
    {

        public RxEntityParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var request = actionContext.Request;
            var stream = request.Content.ReadAsStreamAsync().Result;
            var text = this.GetContentJson(stream); var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());

            actionContext.ActionArguments[Descriptor.ParameterName] = JsonConvert.DeserializeObject(text, setting);
            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }


        private string GetContentJson(Stream stream)
        {
            if (stream.CanSeek)
                stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }
    }

    public class JsonProperyParameterBinding : HttpParameterBinding
    {

        public JsonProperyParameterBinding(HttpParameterDescriptor parameter)
            : base(parameter)
        {
        }

        public override Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
            HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var request = actionContext.Request;
            var ctx = actionContext.Request.GetOwinContext();
            var stream = request.Content.ReadAsStreamAsync().Result;
            var text = this.GetContentJson(stream, ctx);
            var setting = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            var json = JObject.Parse(text);
            var token = json.SelectToken($"$.{Descriptor.ParameterName}");
            if (token is JArray)
            {
                var list = JsonConvert.DeserializeObject(token.ToString(), Descriptor.ParameterType, setting);
                actionContext.ActionArguments[Descriptor.ParameterName] = list;
            }
            else
            {
                actionContext.ActionArguments[Descriptor.ParameterName] = JsonConvert.DeserializeObject(token.ToString(), setting);
            }

            actionContext.ActionArguments[Descriptor.ParameterName] = JsonConvert.DeserializeObject(token.ToString(), setting);
            var tsc = new TaskCompletionSource<object>();
            tsc.SetResult(null);
            return tsc.Task;
        }


        private string GetContentJson(Stream stream, IOwinContext ctx)
        {
            // TODO : we can't read it twice, so attach it to OWIN.context
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
