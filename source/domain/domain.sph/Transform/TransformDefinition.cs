using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace Bespoke.Sph.Domain
{
    public partial class TransformDefinition : Entity
    {
        
        public async Task<object> TransformAsync(object source)
        {
            var sb = new StringBuilder("{");
            sb.AppendLine();
            var tasks = from m in this.MapCollection
                select m.ConvertAsync(source);
            var maps = await Task.WhenAll(tasks);
            sb.AppendLine(string.Join(",\r\n    ", maps.ToArray()));
            sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }

        public async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options, params string[] files)
        {
            await Task.Delay(500);
            if (files.Length == 0)
                throw new ArgumentException("No source files supplied for compilation", "files");
            foreach (var cs in files)
            {
                Debug.WriteLineIf(options.IsVerbose, cs);
            }

            using (var provider = new CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.WorkflowCompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly =
                        Path.Combine(outputPath,
                            string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, this.Name)),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.ReferencedAssemblies.Add(typeof (Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (System.Net.Mail.SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (System.Net.Http.HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (System.Web.HttpResponseBase).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof (ConfigurationManager).Assembly.Location);
                parameters.ReferencedAssemblies.Add(this.InputType.Assembly.Location);
                parameters.ReferencedAssemblies.Add(this.OutputType.Assembly.Location);

                foreach (var es in options.EmbeddedResourceCollection)
                {
                    parameters.EmbeddedResources.Add(es);
                }
                foreach (var ass in options.ReferencedAssembliesLocation)
                {
                    parameters.ReferencedAssemblies.Add(ass);
                }
                var result = provider.CompileAssemblyFromFile(parameters, files);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                var errors = from CompilerError x in result.Errors
                    select new BuildError(this.WebId, x.ErrorText)
                    {
                        Line = x.Line,
                        FileName = x.FileName
                    };
                cr.Errors.AddRange(errors);
                return cr;

            }
        }
    }
}