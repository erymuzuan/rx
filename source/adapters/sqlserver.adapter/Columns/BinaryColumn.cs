using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IncludeTypes = new[] { SqlDbType.VarBinary, SqlDbType.Timestamp, SqlDbType.Binary, SqlDbType.Image },
        IsNullable = ThreeWayBoolean.False)]
    public class BinaryColumn : SqlColumn
    {

    }
}