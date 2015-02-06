using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class SolutionCompiler
    {
        public abstract Task<SphCompilerResult> CompileAsync(Solution solution);
    }
}
