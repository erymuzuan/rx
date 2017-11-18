namespace Bespoke.Sph.Domain
{
    public class BinaryAndFilter : Filter
    {
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
    }
}