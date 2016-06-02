using System.Data;

namespace Bespoke.Sph.Integrations.Adapters
{
    public interface IColumnGeneratorMetadata
    {
        string Name { get; }
        SqlDbType[] IncludeTypes { get; }
        SqlDbType[] ExcludeTypes { get; }
        ThreeWayBoolean IsComputed { get; }
        ThreeWayBoolean IsIdentity { get; }
        ThreeWayBoolean IsNullable { get; }
    }
}