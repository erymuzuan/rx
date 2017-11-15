using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Bespoke.Sph.Domain
{
    public class LoadOperation<T> where T : DomainObject
    {
        public QueryDsl QueryDsl { get; }
        public LoadOperation()
        {

        }

        public LoadOperation(QueryDsl query)
        {
            QueryDsl = query;
        }
        public bool HasError { get; set; }
        public Exception Exception { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public IQueryable<T> Query { get; set; }
        public int? TotalRows { get; set; }
        public int? NextSkipToken { get; set; }

        public ObjectCollection<T> ItemCollection { get; } = new ObjectCollection<T>();
        public ObjectCollection<Dictionary<string, object>> Readers { get; } = new ObjectCollection<Dictionary<string, object>>();

        public TAggregate GetAggregateValue<TAggregate>(string name)
        {
            var agg = QueryDsl?.Aggregates.SingleOrDefault(x => x.Name == name);
            if (agg == null) return default;

            return agg.GetValue<TAggregate>();
        }

        public Dictionary<string, object> Aggregates
        {
            get
            {
                var aggregates = new Dictionary<string, object>();
                if (null == QueryDsl) return aggregates;
                foreach (var agg in QueryDsl.Aggregates)
                {
                    aggregates.Add(agg.Name, this.GetAggregateValue<object>(agg.Name));
                }

                return aggregates;
            }
        }


        public int? TotalPages
        {
            get
            {
                if (PageSize == 0) return null;
                if (null == TotalRows) return null;
                var baki = TotalRows % PageSize;
                var page = (TotalRows - baki) / PageSize;
                return page + 1;
            }

        }
        public bool HasNextPage
        {
            get
            {
                if (null == this.TotalPages) return false;
                return this.CurrentPage < this.TotalPages;
            }

        }
        public bool HasPreviousPage
        {
            get
            {
                if (this.CurrentPage == 1) return false;
                return true;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Items Counts : {0}", this.ItemCollection.Count);
            sb.AppendLine();
            sb.AppendFormat("Next skip : {0}", this.NextSkipToken);
            sb.AppendLine();
            sb.AppendFormat("Total Pages : {0}", this.TotalPages);
            sb.AppendLine();
            sb.AppendFormat("Total Rows : {0}", this.TotalRows);
            sb.AppendLine("Aggregates");
            foreach (var agg in this.Aggregates.Keys)
            {
                sb.AppendLine($"{agg} : {this.Aggregates[agg]}");
            }
            return sb.ToString();
        }
    }
}
