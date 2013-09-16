using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class LineChartItem : ReportItem
    {
        [XmlIgnore]
        [JsonIgnore]
        public string CategoryAxiesValues { get; private set; }


        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            var categoryAxiesValues =
                rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name == this.HorizontalAxisField))
                .Select(c => "'" + c.Value + "'");
            this.CategoryAxiesValues = string.Join(",", categoryAxiesValues);
            
            foreach (var cl in this.ChartSeriesCollection)
            {
                var series1 = cl;
                series1.Values = rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name == series1.Column))
                    .Select(c => Convert.ToDecimal(c.Value)).ToArray();

            }
        }
    }
}
