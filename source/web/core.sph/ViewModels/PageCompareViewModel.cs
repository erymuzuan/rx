namespace Bespoke.Sph.Web.ViewModels
{
    public class PageCompareViewModel
    {
        public string Latest { get; set; }
        public string Old { get; set; }
        public int LogId { get; set; }
        public DiffPlex.DiffBuilder.Model.SideBySideDiffModel Diff { get; set; }
    }
}