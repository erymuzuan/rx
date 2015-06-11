namespace Bespoke.Sph.Web.Models
{
    public class CustomDialog
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }

    }
    public class CustomScript
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }

    }
    public class CustomPartialView
    {
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public bool UseViewModel { get; set; }
    }
}