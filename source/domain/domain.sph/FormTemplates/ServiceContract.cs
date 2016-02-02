using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class ServiceContract : DomainObject
    {
        public Task<WorkflowCompilerResult> CompileAsync(EntityDefinition ed)
        {
            throw new Exception();

        }
    }
}