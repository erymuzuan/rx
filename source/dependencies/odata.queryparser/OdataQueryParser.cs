using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace odata.queryparser
{
    [Export("QueryParser", typeof(IQueryParser))]
    public class OdataQueryParser : IQueryParser
    {
        public QueryDsl Parse(string text)
        {
            var qs = text.Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries);

            string GetQueryStringValue(string key)
            {
                var pair = qs.SingleOrDefault(x => x.StartsWith(key))
                    .ToEmptyString().Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);

                if (pair.Length <= 1)
                    return string.Empty;

                return pair.LastOrDefault().ToEmptyString();
            }


            var filters = ParseFilters(GetQueryStringValue("$filter"));
            var sorts = ParseSorts(GetQueryStringValue("$orderby"));

            var query = new QueryDsl(filters, sorts);


            return query;
        }

        public string Provider => "Odata";
        public string ContentType => "application/odata";

        private Filter[] ParseFilters(string query)
        {
            /* TODO : OdataUri parser
             https://blogs.msdn.microsoft.com/odatateam/2014/07/04/tutorial-sample-using-odatauriparser-for-odata-v4/
             https://github.com/OData/odata.net/tree/ODataV4-6.x for a Odata Uri parser
             */
            var queries = query.Split(new[] {" and ", " or "}, StringSplitOptions.RemoveEmptyEntries);
            var filters = from q in queries
                let words = q.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                let term = words[0]
                let op = (Operator) Enum.Parse(typeof(Operator), words[1], true)
                let value = ParseValue(words)
                select new Filter(term, op, value);

            return filters.ToArray();
        }

        //TODO : when the Field is a function call .e.g "$filter=year(Dob) eq 1950"
        private static object ParseValue(string[] words, int index = 2)
        {
            if (words.Length <= index)
                return null;
            var text = words[index];

            var trim = text.Trim();
            if (trim.StartsWith("'") && trim.EndsWith("'"))
                return trim.Substring(1, trim.Length - 2);
            
            if (trim == "true") return true;
            if (trim == "false") return false;

            /* e.g /Date(694224000000)*/
            if (trim.StartsWith("/Date"))
            {
                var ticksString = Strings.RegexSingleValue(trim, @"/Date\((?<val>[0-9]{10-15})\)", "val");
                if (long.TryParse(ticksString, out var ticks))
                    return new DateTime(1970, 1, 1).AddTicks(ticks);
            }
            if (trim.StartsWith("DateTime"))
            {
                var dateString = Strings.RegexSingleValue(trim, @"DateTime'(?<val>.*?)\'", "val");
                if (DateTime.TryParse(dateString, out var date))
                    return date;
            }

            if (trim.Contains("."))
            {
                if (decimal.TryParse(trim, out var dv))
                    return dv;

            }
            if (int.TryParse(trim, out var iv))
                return iv;
            
            return trim;
        }

        private Sort[] ParseSorts(string odata)
        {
            var queries = odata.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries);
            var sorts = from q in queries
                let words = q.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)
                let path = words[0].Replace("/", ".")
                let direction = (words.Length == 2 && words[1] == "desc") ? SortDirection.Desc : SortDirection.Asc
                select new Sort {Path = path, Direction = direction};

            return sorts.ToArray();
        }
    }
}