using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class SolutionCompiler
    {
        public abstract Task<WorkflowCompilerResult> CompileAsync(Solution solution);
    }
}
