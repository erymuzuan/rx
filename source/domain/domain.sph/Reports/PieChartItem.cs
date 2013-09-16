using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class PieChartItem : ReportItem
    {

        [XmlIgnore]
        [JsonIgnore]
        public string JsonValues { get; private set; }

        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            var data = from r in rows
                       let val = r.ReportColumnCollection.Single(c => c.Name == this.ValueField).Value
                       select new
                       {
                           category = r.ReportColumnCollection.Single(c => c.Name == this.CategoryField).Value,
                           value = Convert.ToDecimal(val)
                       };

            this.JsonValues = JsonConvert.SerializeObject(data);
        }

        private ReportDefinition m_rdl;
        public override void SetRdl(ReportDefinition rdl)
        {
            m_rdl = rdl;
        }
        public string EvaluateTitle()
        {
            if (string.IsNullOrWhiteSpace(this.Title)) return string.Empty;
            if (this.Title.StartsWith("="))
            {
                var engine = ObjectBuilder.GetObject<IScriptEngine>();
                var script = this.Title.Substring(1, this.Title.Length - 1);
                var output = engine.Evaluate<object, Entity>(script, m_rdl);

                return string.Format("{0}", output);
            }

            if (this.Title.StartsWith("@"))
            {
                var output = this.m_rdl.Param(this.Title.Replace("@", string.Empty));
                return string.Format("{0}", output);
            }

            return this.Title;
        }
    }
}
