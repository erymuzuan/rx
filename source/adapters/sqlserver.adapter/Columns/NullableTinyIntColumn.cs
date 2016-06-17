using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.TinyInt }, IsNullable = ThreeWayBoolean.True)]
    public class NullableTinyIntColumn : SqlColumn
    {
        public override SqlDbType SqlType => SqlDbType.TinyInt;
        public override Type ClrType => typeof(byte);

        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullable<byte>()";
        }
    }
}