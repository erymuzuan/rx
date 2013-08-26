using System;
using System.Xml.Serialization;

namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(PieChartItem))]
    [XmlInclude(typeof(LineChartItem))]
    [XmlInclude(typeof(LabelItem))]
    [XmlInclude(typeof(DataGridItem))]
    [XmlInclude(typeof(BarChartItem))]
    [XmlInclude(typeof(BarChartItem))]
    [XmlInclude(typeof(LineItem))]
    public partial class ReportItem : DomainObject
    {
        
        public virtual void SetRows(ObjectCollection<ReportRow> rows)
        {
            Console.WriteLine(rows.Count);
        }
    }
}
