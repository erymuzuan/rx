using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.ViewModels;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class ReportDefinitionController : Controller
    {
        public async Task<ActionResult> Index(int id = 0)
        {
            var context = new SphDataContext();
            var rdl = await context.LoadOneAsync<ReportDefinition>(x => x.ReportDefinitionId == id) ?? new ReportDefinition();

            var items = new ObjectCollection<ReportItem>
            {
                new LabelItem
                {
                    Name = "Label",
                    CssClass = "",
                   Icon = "icon-font"
                },
                new DataGridItem
                {
                    Name = "Table",
                    Icon = "icon-table"
                },
                new LineChartItem
                {
                    Name = "Line chart",
                    Icon = "icon-bar-chart"
                },
                new LineItem
                {
                    Name = "Horizontal line",
                    Icon = "icon-ellipsis-horizontal"
                },
                new PieChartItem
                {
                    Name = "Pie chart",
                    Icon = "icon-circle"
                }
            };

            var vm = new ReportBuilderViewModel
            {
                ReportDefinition = rdl,

            };
            vm.ReportItems.AddRange(items);
            vm.ToolboxItems.AddRange(items);

            return View(vm);
        }

    }
}
