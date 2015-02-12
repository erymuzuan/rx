using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Domain
{
    public interface IProjectProvider
    {
        string DefaultNamespace { get; }
        string Name { get; }
        string Id { get; set; }
        MetadataReference[] GetMetadataReferences();
        Task<IEnumerable<Class>>  GenerateCodeAsync();
        Task<IProjectModel> GetModelAsync();
        Task<SphCompilerResult> CompileAsync(CompilerOptions options);
    }
}