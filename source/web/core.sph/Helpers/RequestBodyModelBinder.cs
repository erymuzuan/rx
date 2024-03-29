﻿using System;
using System.IO;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class RequestBodyAttribute : CustomModelBinderAttribute
    {
        public override IModelBinder GetBinder()
        {
            return new RequestBodyModelBinder();
        }
    }
  

    internal sealed class RequestBodyModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var request = controllerContext.HttpContext.Request;
            var text =this.GetContentJson(request.InputStream);
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            if (bindingContext.ModelType == typeof (string))
                return text;
            return JsonConvert.DeserializeObject(text,bindingContext.ModelType,setting);
        }

        private  string GetContentJson(Stream stream)
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