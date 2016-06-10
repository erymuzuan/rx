using System.Web.Http;
using System.Web.Http.Controllers;

namespace Bespoke.Sph.WebApi
{
    public class UnmodifiedSinceAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(UnmodifiedSinceHeader))
            {
                return new UnmodifiedSinceParameterBinding(parameter);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}