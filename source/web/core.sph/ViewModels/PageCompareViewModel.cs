namespace Bespoke.Sph.Web.ViewModels
{
    public class PageCompareViewModel
    {
        public string Latest { get; set; }
        public string Old { get; set; }
        public string LogId { get; set; }
        public DiffPlex.DiffBuilder.Model.SideBySideDiffModel Diff { get; set; }
    }
}