using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bespoke.Sph.Domain
{
    public partial class DataGridItem : ReportItem
    {
        private ReportDefinition m_rdl;

        public override void SetRdl(ReportDefinition rdl)
        {
            m_rdl = rdl;
        }

        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            this.ReportRowCollection.ClearAndAddRange(rows);
        }

        public IEnumerable<DataGridGroup> GetGroupHeaders()
        {
            var list = new ObjectCollection<DataGridGroup>();
            if (this.DataGridGroupDefinitionCollection.Count == 0) return list;



            foreach (var gf in this.DataGridGroupDefinitionCollection)
            {
                var gf1 = gf;
                var colvalues = this.ReportRowCollection
                                    .Where(r => r[gf1.Column] != null)
                                    .Select(r => r[gf1.Column].Value)
                                    .Where(o => null != o)
                                    .OrderBy(o => o)
                                    .Distinct();
                foreach (var v in colvalues)
                {
                    var v1 = v;
                    var rows = this.ReportRowCollection
                                .Where(r => r[gf1.Column] != null)
                                .Where(r => r[gf1.Column].Value as string == v1 as string).ToList();
                    var group = new DataGridGroup
                    {
                        Column = gf.Column,
                        Text = gf.EvaluateExpression(v1, rows)
                    };
                    group.ReportRowCollection.AddRange(rows);
                    list.Add(group);
                }
            }


            return list;
        }

        public string GetFooterText(string footerExpression)
        {
            if (string.IsNullOrWhiteSpace(footerExpression)) return string.Empty;
            if (footerExpression.StartsWith("="))
            {
                var compiler = ObjectBuilder.GetObject<IScriptEngine>();
                var code = footerExpression.Substring(1, footerExpression.Length - 1);
                var host = new LabelItemScriptHost(this.ReportRowCollection, m_rdl);
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



                return compiler.Evaluate(output, host) as string;


            }

            if (footerExpression.StartsWith("@"))
            {
                var output = this.m_rdl.Param(footerExpression.Replace("@", string.Empty));
                return string.Format("{0}", output);
            }

            return footerExpression;
        }
    }
}
