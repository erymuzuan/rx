using System;
using System.Linq;
using System.Text;


namespace Bespoke.SphCommercialSpaces.Domain
{
    public class LoadOperation<T> where T : DomainObject
    {
        private readonly ObjectCollection<T> m_itemCollection = new ObjectCollection<T>();
        public bool HasError { get; set; }
        public Exception Exception { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IQueryable<T> Query { get; set; }
        public int? TotalRows { get; set; }
        public int? NextSkipToken { get; set; }

        public ObjectCollection<T> ItemCollection
        {
            get { return m_itemCollection; }
        }

        public int? TotalPages
        {
            get
            {
                if(PageSize == 0) return null;
                if(null == TotalRows) return null;
                var baki = TotalRows %PageSize;
                var page = (TotalRows - baki)/PageSize;
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
