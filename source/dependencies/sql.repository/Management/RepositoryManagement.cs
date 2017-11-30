using System.ComponentModel.Composition;
using Bespoke.Sph.Domain.Management;

namespace Bespoke.Sph.SqlRepository.Management
{
    [Export(typeof(IRepositoryManagement))]
    public class RepositoryManagement : SqlServerManagement, IRepositoryManagement
    {

    }
}