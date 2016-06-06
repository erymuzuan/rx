using System;
using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.Int }, IsNullable = ThreeWayBoolean.True)]
    public class NullableIntAndSmallIntColumn : NullableColumn
    {
        public override Type ClrType => typeof(int);
    }
}