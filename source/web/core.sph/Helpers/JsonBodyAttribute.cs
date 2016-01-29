using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Controllers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Helpers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public class JsonBodyAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
            parameter.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            IEnumerable<MediaTypeFormatter> formatters = parameter.Configuration.Formatters;
            var bodyModelValidator = parameter.Configuration.Services.GetBodyModelValidator();
            var ob = parameter.BindWithFormatter(formatters, bodyModelValidator);
            return ob;
        }
    }
}
