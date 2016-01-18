namespace Bespoke.Sph.Domain
{
    public partial class EntityGetOperation : Entity, IEntityDefinitionAsset
    {
        public string Entity { get; }
        public string Icon { get; }
        public string Url { get; }
    }
}