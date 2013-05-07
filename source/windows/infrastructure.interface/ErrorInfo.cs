
namespace Bespoke.Cycling.Windows.Infrastructure
{
    public class ErrorInfo
    {
        public int ErrorCode { get; set; }
        public bool IsWarning { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return (string.Format("{0}: {1}, {2}",
              IsWarning ? "Warning" : "Error",
              ErrorCode,
              ErrorMessage));
        }
    }
}
