using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class FormCompiler : DomainObject
    {
        public abstract Task<SphCompilerResult> CompileAsync(IForm entityForm);
    }
    public abstract class ViewCompiler : DomainObject
    {
        public abstract Task<SphCompilerResult> CompileAsync(EntityView view);
    }
}