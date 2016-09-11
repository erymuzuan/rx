using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiServerAdapter
    {
        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options,
            params string[] namespaces)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new System.NotImplementedException();
        }
    }
}