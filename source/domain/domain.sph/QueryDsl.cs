namespace Bespoke.Sph.Domain
{
    public class QueryDsl
    {
        public QueryDsl(){}

     
        public QueryDsl(Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20)
        {
            if (null != filters)
                Filters.AddRange(filters);
            if (null != sorts)
                Sorts.AddRange(sorts);
            Skip = skip;
            Size = size;
        }

        public int Skip { get; set; }
        public int Size { get; set; } = 20;

        public ObjectCollection<Aggregate> Aggregates { get; } = new ObjectCollection<Aggregate>();
        public ObjectCollection<Filter> Filters { get; } = new ObjectCollection<Filter>();
        public ObjectCollection<Sort> Sorts { get; } = new ObjectCollection<Sort>();
        public ObjectCollection<string> Fields { get; } = new ObjectCollection<string>();

        public override string ToString()
        {
            return $@"
Filters :
    {Filters.ToString("\r\n\t")}";
        }
    }
}