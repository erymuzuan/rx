using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export("SolutionCompiler", typeof(SolutionCompiler))]
    [SolutionCompilerMetadata(Name = "DurandalJs")]
    public class DurandalJsSolutionCompiler : SolutionCompiler
    {
        public override Task<WorkflowCompilerResult> CompileAsync(Solution solution)
        {
            throw new NotImplementedException();
        }
    }
}