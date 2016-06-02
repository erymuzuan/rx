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
        public override string GenerateReadCode()
        {
            return $"item.{this.Name} = reader[\"{this.Name}\"].ReadNullable<long>();";
        }
    }
}