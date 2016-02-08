using System.Text;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Web.Api
{
    public class Sql2008OdataPagingTranslator : IOdataPagingProvider
    {
        public string Tranlate(string sql, int page, int size)
        {
            var predicate = string.Empty;
            const string columnPattern = "^SELECT (?<column>.*?)FROM";
            var columns = this.RegexSingleValue(sql, columnPattern, "column");

            const string tablePattern = " FROM (?<table>.*?) ";
            var table = this.RegexSingleValue(sql, tablePattern, "table");

            if (sql.Contains("WHERE"))
            {
                const string predicatePattern = "WHERE (?<predicate>.*?)ORDER BY";
                predicate = string.Format("WHERE {0}", this.RegexSingleValue(sql, predicatePattern, "predicate"));
            }

            const string orderPattern = "ORDER BY (?<order>.*?)$";
            var order = this.RegexSingleValue(sql, orderPattern, "order");

            var start = (page - 1) * size;
            var end = page * size;

            var output = new StringBuilder("WITH EntityPage AS");
            output.AppendLine();
            output.AppendLine("(");

            var cteColumns = columns;
            var hasCustomOrder = !columns.StartsWith(order);
            if (hasCustomOrder)
            {
                cteColumns += "\r\n," + order.Replace("desc", "").Replace("DESC", "");
            }

            output.AppendLine("SELECT");
            output.AppendLine(cteColumns);
            output.AppendFormat(",ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber ", order);
            output.AppendLine();
            output.AppendLine("FROM");
            output.AppendLine( table);
            output.AppendLine( predicate);
            
            output.AppendLine();
            output.AppendLine(")");
            output.AppendFormat("SELECT {0}", columns);
            output.AppendLine();
            output.AppendLine("FROM EntityPage");
            output.AppendFormat("WHERE RowNumber > {0} AND RowNumber <= {1}", start, end);
            output.AppendLine();
            output.AppendFormat("ORDER BY {0}", order);

            return output.ToString();
        }


        protected string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions option = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, option);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }


    }
}
