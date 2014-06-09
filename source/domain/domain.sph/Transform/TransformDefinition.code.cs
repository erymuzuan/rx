using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition
    {
        private string GetCodeHeader()
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(Int32).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        public Dictionary<string, string> GenerateCode()
        {
            var header = this.GetCodeHeader();
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name);
            code.AppendLine("   {");


            code.AppendLine(this.GenerateTransformCode());
            code.AppendLine(this.GenerateTransformToArrayCode());


            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace

            var sourceCodes = new Dictionary<string, string> { { this.Name + ".cs", code.ToString() } };


            return sourceCodes;
        }


        public string[] SaveSources(Dictionary<string, string> sources)
        {
            var folder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, this.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources.Keys)
            {
                var file = Path.Combine(folder, cs);
                File.WriteAllText(file, sources[cs]);
            }
            return sources.Keys.ToArray()
                    .Select(f => string.Format("{0}\\{1}\\{2}", ConfigurationManager.WorkflowSourceDirectory, this.Name, f))
                    .ToArray();
        }
        public string CodeNamespace
        {
            get { return string.Format("{0}.Integrations.Transforms", ConfigurationManager.ApplicationName); }
        }

        public string GenerateTransformCode()
        {
            throw new Exception("TODO");
        }

        public string GenerateTransformToArrayCode()
        {
            throw new Exception("return the list of destinations objects");
        }

    }
}