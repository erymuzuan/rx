using System;
using System.Text;
using System.Text.RegularExpressions;

namespace __NAMESPACE__
{
    public class OraclePagingTranslator
    {
        public string Translate(string sql, int page, int size)
        {
            var predicate = string.Empty;
            const string COLUMN_PATTERN = "^SELECT (?<column>.*?)FROM";
            var columns = this.RegexSingleValue(sql, COLUMN_PATTERN, "column");


            const string TABLE_PATTERN = "FROM (?<table>.*?) ";
            var table = this.RegexSingleValue(sql, TABLE_PATTERN, "table");
            if (columns.Trim() == "*")
                table += " a"; //alias

            if (sql.Contains("WHERE"))
            {
                const string PREDICATE_PATTERN = "WHERE (?<predicate>.*?)ORDER";
                predicate = string.Format("WHERE {0}", this.RegexSingleValue(sql, PREDICATE_PATTERN, "predicate"));
            }

            const string ORDER_PATTERN = "ORDER BY (?<order>.*?)$";
            var order = this.RegexSingleValue(sql, ORDER_PATTERN, "order");

            var start = (page - 1) * size;
            var end = page * size;

            var code = new StringBuilder("SELECT ");
            code.AppendLine(columns);
            code.AppendLine("FROM");
            code.AppendLine("(");

            var cteColumns = columns;
            var hasCustomOrder = !columns.StartsWith(order);
            if (hasCustomOrder)
            {
                cteColumns += "\r\n," + order.Replace("DESC", "");
            }

            code.AppendLine("SELECT");
            code.AppendLine(columns.Trim() == "*" ? "a.* " : cteColumns);
            code.AppendFormat(",ROW_NUMBER() OVER(ORDER BY {0}) AS RowNumber ", order);
            code.AppendLine();
            code.AppendLine("FROM");
            code.AppendLine(table);
            code.AppendLine(predicate);

            code.AppendLine();
            code.AppendLine(")");
            code.AppendFormat("WHERE RowNumber > {0} AND RowNumber <= {1}", start, end);
            code.AppendLine();
            code.AppendFormat("ORDER BY {0}", order);

            return code.ToString();
        }


        protected string RegexSingleValue(string input, string pattern, string group)
        {
            const RegexOptions OPTIONS = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            var matches = Regex.Matches(input, pattern, OPTIONS);
            return matches.Count == 1 ? matches[0].Groups[@group].Value.Trim() : null;
        }

    }
}
