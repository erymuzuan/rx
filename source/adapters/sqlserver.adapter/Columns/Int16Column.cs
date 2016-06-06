using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.TinyInt, SqlDbType.SmallInt }, IsNullable = ThreeWayBoolean.False)]
    public class TinyIntColumn : NonNullableColumn
    {
        public override SqlDbType SqlType => SqlDbType.TinyInt;
        public override Type ClrType => typeof(short);
    }
}