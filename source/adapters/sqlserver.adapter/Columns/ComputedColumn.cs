using System.ComponentModel.Composition;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsComputed = ThreeWayBoolean.True)]
    public class ComputedColumn : SqlColumn
    {
        public override bool CanWrite => false;
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            return null;
        }
    }
}