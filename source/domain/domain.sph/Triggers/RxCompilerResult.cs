using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public class RxCompilerResult
    {
        public static Task<RxCompilerResult> TaskEmpty = Task.FromResult(new RxCompilerResult { Result = true, IsEmpty = true });
        public static RxCompilerResult Empty = new RxCompilerResult { Result = true, IsEmpty = true };
        public bool IsEmpty { get; private set; }

        public bool Result { get; set; }
        public ObjectCollection<BuildError> Errors { get; } = new ObjectCollection<BuildError>();

        public override string ToString()
        {
            if (this.Result) return "===== Build: 1 succeeded";
            var message = new StringBuilder();
            message.AppendLine("====================== " + (this.Result ? "Success" : "Failed") + " =====================");
            foreach (var error in Errors)
            {
                message.AppendLine(error.Message);
                if (!string.IsNullOrWhiteSpace(error.FileName))
                    message.AppendLine($"{error.FileName}:{error.Line}");
            }
            return message.ToString();
        }

        public string Output { get; set; }
    }
}