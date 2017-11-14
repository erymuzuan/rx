namespace Bespoke.Sph.Domain
{
    public class BinaryOrFilter : Filter
    {
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
        public override string ToString()
        {
            return $"({Filters.ToString(" OR\r\n\t", x => x.ToString())})";
        }
    }
}