using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public interface IEndpointsBuilder
    {
        Task<RestApiOperationDefinition> BuildAsync();
        double Order { get; }
        string StoreId { get; set; }
        Task<ValidationResult> ValidateAsync();
        Task<IEnumerable<IEndpointsBuilder>> GetBuildersAsync();
        Task<IEnumerable<Member>> GetRequestBodyMembersAsync();
        Task<IEnumerable<Member>> GetRequestHeaderMembersAsync();
        Task<IEnumerable<Member>> GetRequestQueryStringMembersAsync();

        Task<IEnumerable<Member>> GetResponseBodyMembersAsync();
        Task<IEnumerable<Member>> GetResponseHeaderMembersAsync();
    }
}