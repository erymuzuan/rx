using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public abstract class SqlOperationDefinition : OperationDefinition
    {
        public string ObjectType { get; set; }
        public abstract Task InitializeRequestMembersAsync(SqlServerAdapter adapter);
        protected override string GenerateAdapterActionBody(Adapter adapter)
        {
            throw new Exception("Should be implemented by one of the function or sprocs");
        }

        public override string ToString()
        {
            return $"[{Schema}].[{Name}]";
        }
    }
}