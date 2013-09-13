using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class BarChartItem : ReportItem
    {

        [XmlIgnore]
        [JsonIgnore]
        public string CategoryAxiesValues { get; private set; }

        [XmlIgnore]
        [JsonIgnore]
        public ChartSeries[] Series { get; set; }

        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            var categoryAxiesValues =
                rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name == this.HorizontalAxisField))
                .Select(c => "'" + c.Value + "'");
            this.CategoryAxiesValues = string.Join(",", categoryAxiesValues);

            var series = new ObjectCollection<ChartSeries>();
            var columns = rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name != this.HorizontalAxisField))
                .Select(c => c.Name).Distinct();

            foreach (var cl in columns)
            {
                var cl1 = cl;
                var sr = new ChartSeries
                {
                    Header = cl,
                    Values = rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name == cl1))
                        .Select(c => Convert.ToDecimal(c.Value)).ToArray()
                };
                series.Add(sr);
            }
            this.Series = series.ToArray();
        }
    }
}
