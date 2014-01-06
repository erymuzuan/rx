using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class ReportBuilderViewModel
    {
        public ReportBuilderViewModel()
        {
            this.ToolboxItems.Add(new LabelItem { Name = "Label", CssClass = "icon-report-label" });
            this.ToolboxItems.Add(new LineItem { Name = "Line", CssClass = "icon-report-line" });
            this.ToolboxItems.Add(new DataGridItem { Name = "Table", CssClass = "icon-report-table" });
            this.ToolboxItems.Add(new LineChartItem { Name = "Line Chart", CssClass = "icon-report-linechart" });
            this.ToolboxItems.Add(new BarChartItem { Name = "Bar Chart", CssClass = "icon-report-barchart" });
            this.ToolboxItems.Add(new PieChartItem { Name = "Pie Chart", CssClass = "icon-report-piechart" });
        }

        //public ReportDefinition ReportDefinition { set; get; }
        private readonly ObjectCollection<ReportItem> m_toolboxItemCollection = new ObjectCollection<ReportItem>();
        public ObjectCollection<ReportItem> ToolboxItems
        {
            get { return m_toolboxItemCollection; }
        }

        private readonly ObjectCollection<ReportItem> m_reportItemCollection = new ObjectCollection<ReportItem>();
        public ObjectCollection<ReportItem> ReportItems
        {
            get { return m_reportItemCollection; }
        }

    }
}