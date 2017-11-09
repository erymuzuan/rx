using System;

namespace Bespoke.Sph.Domain
{
    public class QueryDsl
    {
        public QueryDsl()
        {

        }

        public QueryDsl(string filters, string sorts = "", string projections = "")
        {
            var rxFilters = Filter.Parse(filters.ToEmptyString());
            Filters.AddRange(rxFilters);

            var rxSorts= Sort.Parse(sorts.ToEmptyString());
            Sorts.AddRange(rxSorts);
            Fields.AddRange(projections.ToEmptyString().Split(new []{",", ";"},StringSplitOptions.RemoveEmptyEntries));
        }

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
    }
}