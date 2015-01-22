using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class FormCompiler : DomainObject
    {
        public abstract Task<WorkflowCompilerResult> CompileAsync(IForm entityForm);
    }
}