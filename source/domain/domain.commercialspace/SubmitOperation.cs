using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public class SubmitOperation
    {
        public SubmitOperation()
        {
            Books = new Dictionary<string, int>();
        }
        private Dictionary<string, int> m_books = new Dictionary<string, int>();

        public Exception Exeption { get; set; }
        public int RowsAffected { get; set; }
        public bool IsFaulted { get { return null != this.Exeption; } }
        public bool IsCompleted { get { return this.RowsAffected > 0; } }

        public Dictionary<string, int> Books
        {
            get { return m_books; }
            set { m_books = value; }
        }

        public bool Add(string webId, int id)
        {
            if (Books.ContainsKey(webId)) return false;

            Books.Add(webId, id);
            return true;
        }

        public int? Get(string webId)
        {
            if (Books.ContainsKey(webId))
                return Books[webId];

            return null;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("SubmitOperation :{0} rows", this.RowsAffected);
            foreach (var key in Books.Keys)
            {
                sb.AppendLine();
                sb.AppendFormat("{0,-20}:{1}", key, Books[key]);
            }
            return sb.ToString();
        }

        public string SerializeToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
