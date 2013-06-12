namespace Bespoke.Sph.SqlRepository
{
    public interface ISqlServerMetadata
    {
        Table GetTable(string name);
    }
}