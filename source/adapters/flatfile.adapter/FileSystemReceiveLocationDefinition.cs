using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(ReceivePortDefinition))]
    [Export(typeof(ReceivePortDefinition))]
    public class FileSystemReceiveLocationDefinition : ReceivePortDefinition
    {
        public string Directory { get; set; }
        public string Filter { get; set; }

        private string GetCodeHeader(params string[] namespaces)
        {
            var header = new StringBuilder();
            header.AppendLine("using " + typeof(Entity).Namespace + ";");
            header.AppendLine("using " + typeof(int).Namespace + ";");
            header.AppendLine("using " + typeof(Task<>).Namespace + ";");
            header.AppendLine("using " + typeof(Enumerable).Namespace + ";");
            header.AppendLine("using " + typeof(IEnumerable<>).Namespace + ";");
            header.AppendLine("using " + typeof(Encoding).Namespace + ";");
            header.AppendLine("using " + typeof(CookieContainer).Namespace + ";");
            header.AppendLine("using " + typeof(Directory).Namespace + ";");
            header.AppendLine("using " + typeof(XmlAttributeAttribute).Namespace + ";");
            header.AppendLine("using " + typeof(FileHelpers.FieldFixedLengthAttribute).Namespace + ";");
            header.AppendLine("using System.Web.Mvc;");
            header.AppendLine("using Bespoke.Sph.Web.Helpers;");
            foreach (var ns in namespaces)
            {
                header.AppendLinf("using {0};", ns);
            }
            header.AppendLine();

            header.AppendLine("namespace " + this.CodeNamespace);
            header.AppendLine("{");
            return header.ToString();

        }

        protected override Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            var sources = new Dictionary<string, string>();

            var header = this.GetCodeHeader(namespaces);
            var code = new StringBuilder(header);

            code.AppendLine("   public class " + this.Name + " : ReceiveLocation");
            code.AppendLine("   {");

            code.AppendLine("       public string Directory {get; set;}");
            code.AppendLine("       public string Filter {get; set;}");
            code.AppendLine("       private FileSystemWatcher m_watcher;");
            code.AppendLine("       public void Start()");
            code.AppendLine("       {");
            code.AppendLine("           if(null != m_watcher) m_watcher.Dispose();");
            code.AppendLine("           m_watcher = new FileSystemWatcher(this.Directory, this.Filter) {EnableRaisingEvents = true};");
            code.AppendLine("           m_watcher.Created += (e, a) => { };");
            code.AppendLine("       }");

       

            code.AppendLine("   }");// end class
            code.AppendLine("}");// end namespace
            sources.Add(this.Name + ".cs", code.ToString());

            return Task.FromResult(sources);
        }
    }
}
