using System.ComponentModel.Composition;
using Bespoke.Sph.Domain.Management;

namespace Bespoke.Sph.SqlRepository.Management
{
    [Export(typeof(IReadOnlyRepositoryManagement))]
    public class ReadOnlyRepositoryManagement : SqlServerManagement, IReadOnlyRepositoryManagement
    {
    }
}
