namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class DataGridItem : ReportItem
    {
        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            this.ReportRowCollection.ClearAndAddRange(rows);
        }
    }
}
