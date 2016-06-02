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
        public override Type ClrType => typeof(short);

        public override string GenerateReadCode()
        {
            return $"item.{this.Name} = reader[\"{this.Name}\"].ReadNullable<short>();";
        }
    }
}