using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ICompilationUnit
    {
        Task<WorkflowCompilerResult> CompileAsync(EntityDefinition entityDefinition);
        string CodeNamespace { get; }
        string AssemblyName { get; }
        string PdbName { get; }
        string TypeName { get; }
        string TypeFullName { get; }
        string Name { set; get; }
        bool IsPublished { set; get; }

        IBuildDiagnostics[] BuildDiagnostics { get; set; }
        Task<ValidationResult> ValidateAsync<T>(T item, EntityDefinition ed) where T : Entity;
    }
}