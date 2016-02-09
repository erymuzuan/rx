using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Metadata;

namespace Bespoke.Sph.WebApi
{
    /// <summary>
    /// An attribute that captures the entire content body and stores it
    /// into the parameter of type string or byte[].
    /// </summary>
    /// <remarks>
    /// The parameter marked up with this attribute should be the only parameter as it reads the
    /// entire request body and assigns it to that parameter.    
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter)]
    public sealed class RawBodyAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter.ParameterType == typeof(string) || parameter.ParameterType == typeof(byte[]))
            {
                return new RawBodyParameterBinding(parameter, parameter.ParameterType);
            }
            return parameter.BindAsError("Wrong parameter type, only string or byte[]");
        }
    }
    /// <summary>
    /// Reads the Request body into a string/byte[] and
    /// assigns it to the parameter bound.
    /// 
    /// Should only be used with a single parameter on
    /// a Web API method using the [NakedBody] attribute
    /// </summary>
    public class RawBodyParameterBinding : HttpParameterBinding
    {
        private readonly Type m_type;

        public RawBodyParameterBinding(HttpParameterDescriptor descriptor, Type type)
            : base(descriptor)
        {
            m_type = type;
        }


        public override async Task ExecuteBindingAsync(ModelMetadataProvider metadataProvider,
                                                    HttpActionContext actionContext,
                                                    CancellationToken cancellationToken)
        {
            if (actionContext.Request.Method == HttpMethod.Get)
                throw new Exception("You cannot bind GET action to Raw body");


            if (m_type == typeof(string))
            {
                var body = await actionContext.Request.Content.ReadAsStringAsync();
                actionContext.ActionArguments[Descriptor.ParameterName] = body;
            }
            else if (m_type == typeof(byte[]))
            {
                var raw = await actionContext.Request.Content.ReadAsByteArrayAsync();
                actionContext.ActionArguments[Descriptor.ParameterName] = raw;
            }

        }

        public override bool WillReadBody => true;
    }

}
