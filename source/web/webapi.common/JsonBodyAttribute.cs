using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;
using Newtonsoft.Json;

namespace Bespoke.Sph.WebApi
{

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
            var text = this.GetContentJson(stream);
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

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
}
