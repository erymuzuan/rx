namespace Bespoke.Sph.Domain
{
    public class CompoundAndFilter : Filter
    {
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
    }
}