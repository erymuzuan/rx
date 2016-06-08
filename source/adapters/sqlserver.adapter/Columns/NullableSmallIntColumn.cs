using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.SmallInt }, IsNullable = ThreeWayBoolean.True)]
    public class NullableSmallIntColumn : NullableColumn
    {
        public override Type ClrType => typeof(short);
    }
}