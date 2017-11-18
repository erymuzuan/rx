namespace Bespoke.Sph.Domain
{
    public class GroupByAggregate : Aggregate
    {
        public GroupByAggregate(string name, string path) : base(name, path)
        {

        }
    }

    public class DateHistogramAggregate : GroupByAggregate
    {
        public string Interval { get; }

        public DateHistogramAggregate(string name, string path, string interval) : base(name, path)
        {
            Interval = interval;
        }

    }
}