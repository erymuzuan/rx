namespace Bespoke.Sph.Domain
{
    public class Query
    {
        public Query()
        {

        }
        public Query(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20)
        {
            Filters.AddRange(filters);
            Sorts.AddRange(sorts);
            Skip = skip;
            Size = size;
        }

        public int Skip { get; set; } = 0;
        public int Size { get; set; } = 20;

        public ObjectCollection<Aggregate> Aggregates { get; } = new ObjectCollection<Aggregate>();
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
        public ObjectCollection<Sort> Sorts { get; } = new ObjectCollection<Sort>();
        public ObjectCollection<string> Fields { get; } = new ObjectCollection<string>();
    }
}