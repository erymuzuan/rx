using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Timestamp }, IsNullable = ThreeWayBoolean.False)]
    public class RowVersionColumn : SqlColumn
    {
        public override bool CanWrite => false;
        public override Type ClrType => typeof(byte[]);
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return null;
        }
        
        public override string GenerateValueAssignmentCode(string dbValue)
        {
            return $"{dbValue}.ReadNullableByteArray()";
        }
    }
}