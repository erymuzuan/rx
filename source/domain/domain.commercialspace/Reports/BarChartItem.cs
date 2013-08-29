using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class BarChartItem : ReportItem
    {

        [XmlIgnore]
        [JsonIgnore]
        public string JsonValues { get; private set; }
        [XmlIgnore]
        [JsonIgnore]
        public string CategoryAxiesValues { get; private set; }

        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            var categoryAxiesValues =
                rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name == this.HorizontalAxisField))
                .Select(c => "'" + c.Value + "'");
            this.CategoryAxiesValues = string.Join(",", categoryAxiesValues);

            var a =
                rows.SelectMany(r => r.ReportColumnCollection.Where(c => c.Name != this.HorizontalAxisField))
                .Select(c => c.Value);
            this.JsonValues = string.Join(",", a);
        }
    }
}
