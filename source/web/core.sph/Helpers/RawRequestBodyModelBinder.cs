using System;
using System.IO;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class RawRequestBodyAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new RawRequestBodyModelBinder();
        }
    }

    internal sealed class RawRequestBodyModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var text  =GetRawString(request.InputStream);
            return text;
        }

        private static string GetRawString(Stream stream)
        {
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                string text = reader.ReadToEnd();
                return text;
            }
        }
    }
}