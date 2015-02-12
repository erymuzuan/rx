using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Formatting;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Helpers
{
    public static class ApiHelpers
    {
        /// <summary>
        /// Provides temporary method for deserializing request stream, once we got the ParamterBinding then this method will cease to exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controller"></param>
        /// <param name="item">The parameter</param>
        /// <param name="test"></param>
        /// <returns></returns>
        public static T GetRequestBody<T>(this ApiController controller, T item, Func<T, bool> test)
        {
            if (null != item && test(item)) return item;

            if (null == controller.Request) return default(T);
            using (var stream = new MemoryStream())
            {
                var context = (HttpContextBase)controller.Request.Properties["MS_HttpContext"];
                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                context.Request.InputStream.CopyTo(stream);
                var requestBody = Encoding.UTF8.GetString(stream.ToArray());

                return requestBody.DeserializeFromJson<T>();
            }
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public sealed class FromBody2Attribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException("parameter");
            }

            IEnumerable<MediaTypeFormatter> formatters = parameter.Configuration.Formatters;
            var validator = parameter.Configuration.Services.GetBodyModelValidator();

            return parameter.BindWithFormatter(formatters, validator);
        }
    }
}