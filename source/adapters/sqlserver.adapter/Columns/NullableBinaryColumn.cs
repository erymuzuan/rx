using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.VarBinary, SqlDbType.Timestamp, SqlDbType.Binary, SqlDbType.Image },
        IsNullable = ThreeWayBoolean.True)]
    public class NullableBinaryColumn : SqlColumn
    {
        public override Type ClrType => typeof(byte[]);
        public override string GenerateReadCode()
        {
            return $"item.{Name} = reader[\"{Name}\"].ReadNullableByteArray();";
        }
    }
}