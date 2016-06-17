using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsNullable = ThreeWayBoolean.True)]
    public class NullableColumn : SqlColumn
    {
        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullable<{ClrType.ToCSharp()}>()";
        }
    }
}