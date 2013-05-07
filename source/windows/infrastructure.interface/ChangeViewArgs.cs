
namespace Bespoke.Cycling.Windows.Infrastructure
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
