using System.Text;

namespace Bespoke.Sph.Domain
{
    public class WorkflowCompilerResult
    {
        public bool Result { get; set; }
        public ObjectCollection<BuildError> Errors { get; } = new ObjectCollection<BuildError>();

        public override string ToString()
        {
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