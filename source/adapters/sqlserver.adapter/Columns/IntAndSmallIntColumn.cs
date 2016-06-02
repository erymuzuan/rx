using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Int, SqlDbType.SmallInt }, IsNullable = ThreeWayBoolean.False)]
    public class IntAndSmallIntColumn : NonNullableColumn
    {
        public override Type ClrType => typeof(int);
    }
}