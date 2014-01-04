using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class RdlExecutionViewModel
    {
        public ReportDefinition Rdl { get; set; }
        public bool IsPostback { get; set; }
    }
}
