using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.BigInt }, IsNullable = ThreeWayBoolean.True)]
    public class NullableInt64Column : SqlColumn
    {
        public override Type ClrType => typeof(long);

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullable<long>()";
        }
    }
}