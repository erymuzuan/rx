using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class SubmitOperation
    {
        public SubmitOperation()
        {
            Books = new Dictionary<string, string>();
        }

        public Exception Exeption { get; set; }
        public int RowsAffected { get; set; }
        public bool IsFaulted => null != this.Exeption;
        public bool IsCompleted => this.RowsAffected > 0;
        public string Token { get; set; }

        public Dictionary<string, string> Books { get; set; }

        public bool Add(string webId, string id)
        {
            if (Books.ContainsKey(webId)) return false;

            Books.Add(webId, id);
            return true;
        }

        public string Get(string webId)
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
