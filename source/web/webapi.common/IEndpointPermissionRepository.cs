using System.Collections.Generic;

namespace Bespoke.Sph.WebApi
{
    public interface IEndpointPermissionRepository
    {
        IEnumerable<EndpointPermissonSetting> Settings { get; }
    }
}