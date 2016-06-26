using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsIdentity = ThreeWayBoolean.True)]
    public class IdentityColumn : SqlColumn
    {
        public override bool CanWrite => false;
        public override string GenerateUpdateParameterValue(string commandName = "cmd", string itemIdentifier = "item")
        {
            return null;
        }

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"({this.ClrType.ToCSharp()}){dbValue}";
        }
    }
}