using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public partial class DataGridColumn : DomainObject
    {
        public string GenerateHtml(ReportRow row)
        {
            if (string.IsNullOrWhiteSpace(this.Expression)) return string.Empty;

            const string pattern = @"\[(?<column>.*?)\]";
            if (this.Expression.StartsWith("="))
            {
                var compiler = ObjectBuilder.GetObject<IScriptEngine>();
                var code = this.Expression.Substring(1, this.Expression.Length - 1); ;
                var host = new DataGridColumnHost { Row = row, ColumnDefinition = this };

                var output = Regex.Replace(code, pattern, m => string.Format("item.ColumnValue(\"{0}\")", m.Groups["column"].Value));
                return compiler.Evaluate(output, host) as string;
            }

            var col = Strings.RegexSingleValue(this.Expression, pattern, "column");
            if (!string.IsNullOrWhiteSpace(col))
            {
                var f = row[col];
                if (null != f && !string.IsNullOrWhiteSpace(this.Format)) return string.Format(this.Format, f.Value);
                if (null != f && string.IsNullOrWhiteSpace(this.Format)) return f.Value.ToEmptyString();
                return string.Empty;
            }
            return row.ToString();
        }
    }

    public class DataGridColumnHost : Entity, ICustomScript
    {
        public ReportRow Row { get; set; }
        public DataGridColumn ColumnDefinition { get; set; }

        public string ColumnValue(string column)
        {
            var f = this.Row[column];
            return null != f ? f.Value.ToEmptyString() : string.Empty;
        }

        public string Script
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
