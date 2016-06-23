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
            if (ClrType.IsValueType)
                return $"{dbValue}.ReadNullable<{ClrType.ToCSharp()}>()";
            return $"{dbValue}.ReadNullableObject<{ClrType.ToCSharp()}>()";
        }
    }
}