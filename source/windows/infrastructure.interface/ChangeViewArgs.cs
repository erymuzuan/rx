
namespace Bespoke.Sph.Windows.Infrastructure
{
    public class ChangeViewArgs
    {
        public string ViewName { get; set; }

        public ChangeViewArgs(string viewName)
        {
            this.ViewName = viewName;
        }
    }
}
