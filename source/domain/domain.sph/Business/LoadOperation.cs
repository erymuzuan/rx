using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Bespoke.Sph.Domain
{
    public class LoadOperation<T> where T : DomainObject
    {
        public bool HasError { get; set; }
        public Exception Exception { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public string Filter { get; set; }
        public IQueryable<T> Query { get; set; }
        public int? TotalRows { get; set; }
        public int? NextSkipToken { get; set; }

        public ObjectCollection<T> ItemCollection { get; } = new ObjectCollection<T>();
        public ObjectCollection<Dictionary<string, object>> Readers { get; } = new ObjectCollection<Dictionary<string,object>>();

        public TAggregate GetAggregateValue<TAggregate>(string name)
        {
            return default;
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
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
