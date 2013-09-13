using System.Collections.Generic;
using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class DataGridGroupDefinition : DomainObject
    {
        public string GenerateHtml(IEnumerable<ReportRow> rows )
        {
            if (string.IsNullOrWhiteSpace(this.Expression)) return string.Empty;

            //const string pattern = @"\[(?<column>.*?)\]";
            //if (this.Expression.StartsWith("="))
            //{
            //    var compiler = ObjectBuilder.GetObject<IScriptEngine>();
            //    var code = this.Expression.Substring(1, this.Expression.Length - 1); ;
            //    var host = new DataGridColumnHost { Row = row, ColumnDefinition = this };

            //    var output = Regex.Replace(code, pattern, m => string.Format("item.ColumnValue(\"{0}\")", m.Groups["column"].Value));
            //    return compiler.Evaluate(output, host) as string;
            //}

            //var col = Strings.RegexSingleValue(this.Expression, pattern, "column");
            //if (!string.IsNullOrWhiteSpace(col))
            //{
            //    var f = row[col];
            //    if (null != f && !string.IsNullOrWhiteSpace(this.Format)) return string.Format(this.Format, f.Value);
            //    if (null != f && string.IsNullOrWhiteSpace(this.Format)) return f.Value.ToEmptyString();
            //    return string.Empty;
            //}
            //return row.ToString();
            return string.Empty;
        }

        public string EvaluateExpression(object value, IEnumerable<ReportRow> rows)
        {
            return value.ToEmptyString() + " COUNT:" + rows.Count();
        }
    }
}