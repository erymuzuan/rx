using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters.Columns
{
    [Export("SqlColumn", typeof(SqlColumn))]
    [ColumnGeneratorMetadata(IsNullable = ThreeWayBoolean.False)]
    public class NonNullableColumn : SqlColumn
    {
        public override string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }
    }
}