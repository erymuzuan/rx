using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiOperationDefinition : OperationDefinition
    {
        private readonly IEndpointsBuilder m_builder;

        public RestApiOperationDefinition()
        {

        }

        public RestApiOperationDefinition(IEndpointsBuilder builder)
        {
            m_builder = builder;
        }

        public string HttpMethod { get; set; }
        public string BaseAddress { get; set; }
    }
}
