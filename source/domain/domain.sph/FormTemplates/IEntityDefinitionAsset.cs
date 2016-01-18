namespace Bespoke.Sph.Domain
{
    public interface IEntityDefinitionAsset
    {
        string Id { get; }
        string Name { get; }
        string Entity { get; }
        string Icon { get; }
        string Url { get; }
    }
}