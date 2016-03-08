using System;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Bespoke.Sph.WebApi
{
    public class ModifiedSinceAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(ModifiedSinceHeader))
            {
                return new ModifiedSinceParameterBinding(parameter);
            }
            return parameter.BindAsError("Wrong parameter type");
        }
    }
}