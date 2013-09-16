using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public partial class LabelItem : ReportItem
    {
        private ReportDefinition m_rdl;
        private ObjectCollection<ReportRow> m_rows;

        public override void SetRdl(ReportDefinition rdl)
        {
            m_rdl = rdl;
        }

        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            this.m_rows = rows;
        }

        public string EvaluateHtml()
        {
            if (string.IsNullOrWhiteSpace(this.Html)) return string.Empty;
            if (this.Html.StartsWith("="))
            {
                var compiler = ObjectBuilder.GetObject<IScriptEngine>();
                var code = this.Html.Substring(1, this.Html.Length - 1);
                var host = new LabelItemScriptHost(m_rows, m_rdl);
                const string pattern = @"(?<aggregate>[A-Z]{3})\(\[(?<column>.*?)\]\s?\)";
                const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Singleline;

                var output = Regex.Replace(code, pattern, m =>
                {
                    var agg = m.Groups["aggregate"].Value;
                    if (agg == "SUM") agg = "Sum";
                    if (agg == "AVG") agg = "Average";
                    if (agg == "MIN") agg = "Min";
                    if (agg == "MAX") agg = "Max";
                    var c = string.Format("item.{0}(\"{1}\")", agg, m.Groups["column"].Value);
                    return c;
                }, options);

                const string parameterPattern = "@(?<param>[A-Za-z0-9_]{1,100})";
                const string parameterCode = "item.Param(\"{0}\")";
                output = Regex.Replace(output, parameterPattern, m =>
                {
                    var pname = m.Groups["param"].Value;
                    if (m_rdl.DataSource.ParameterCollection.Any(p => p.Name == pname))
                        return string.Format(parameterCode, pname);
                    return pname;

                }, options);



                return compiler.Evaluate<string, Entity>(output, host);


            }

            if (this.Html.StartsWith("@"))
            {
                var output = this.m_rdl.Param(this.Html.Replace("@", string.Empty));
                return string.Format("{0}", output);
            }

            return this.Html;
        }

    }
}
