using System.ComponentModel.Composition;
using System.Data;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsComputed = ThreeWayBoolean.True, IncludeTypes = new[] { SqlDbType.VarChar, SqlDbType.Char, SqlDbType.NChar, SqlDbType.NVarChar, SqlDbType.Text, SqlDbType.NText, })]
    public class ComputedStringColumn : StringColumn
    {
        public override bool CanWrite => false;
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return null;
        }

    }
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsComputed = ThreeWayBoolean.True, IncludeTypes = new[] { SqlDbType.Int })]
    public class ComputedIntColumn : IntColumn
    {
        public override bool CanWrite => false;
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return null;
        }

    }
}