using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Bespoke.Sph.WebApi
{
    public class ContentTypeAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(MediaTypeHeaderValue))
            {
                return new ContentTypeParameterBinding(parameter);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}