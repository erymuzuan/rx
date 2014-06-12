using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter
    {
        public string[] SaveSources(Dictionary<string, string> sources, string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources.Keys)
            {
                var file = Path.Combine(folder, cs);
                File.WriteAllText(file, sources[cs]);
            }
            return sources.Keys.ToArray()
                    .Select(f => Path.Combine(folder, f))
                    .ToArray();
        }

        public WorkflowCompilerResult Compile(CompilerOptions options, params string[] files)
        {
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
                    OutputAssembly = Path.Combine(outputPath, string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, this.Name)),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Mail.SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Http.HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Web.HttpResponseBase).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(ConfigurationManager).Assembly.Location);

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

        public async Task<WorkflowCompilerResult> CompileAsync()
        {

            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            options.AddReference(typeof(System.Data.UpdateStatus));
            options.AddReference(typeof(System.Configuration.ConfigurationManager));

            var sourceFolder = Path.Combine(ConfigurationManager.WorkflowSourceDirectory, this.Name);
            var sources = new List<string>();

            foreach (var table in this.Tables)
            {
                var td = await this.GetSchemaDefinitionAsync(table.Name);
                td.CodeNamespace = this.CodeNamespace;
                var es = string.Format("{0}.{1}.schema.json", this.Name.ToLowerInvariant(), table);
                File.WriteAllText(es, td.ToJsonString(true));
                options.EmbeddedResourceCollection.Add(es);
                var codes = td.GenerateCode(this);
                var tdSources = this.SaveSources(codes, sourceFolder);
                sources.AddRange(tdSources);

            }

            var adapterCodes = await this.GenerateSourceCodeAsync(options, this.CodeNamespace);
            var adapterSources = this.SaveSources(adapterCodes, sourceFolder);
            
            var odataTranslatorCode = await this.GenerateOdataTranslatorSourceCodeAsync();
            var odataSource = this.SaveSources(new Dictionary<string, string>
            {
                {
                    odataTranslatorCode.Item1,
                    odataTranslatorCode.Item2
                }
            }, sourceFolder);
            sources.AddRange(odataSource);
            
            var pagingCode = await this.GeneratePagingSourceCodeAsync();
            var pagingSource = this.SaveSources(new Dictionary<string, string>
            {
                {
                    pagingCode.Item1,
                    pagingCode.Item2
                }
            }, sourceFolder);
            sources.AddRange(pagingSource);


            var result = this.Compile(options, sources.ToArray());

            if (!result.Result)
                throw new Exception(string.Join("\r\n", result.Errors.Select(e => e.ToString())));

            return result;

        }

        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync();
        protected abstract Task<Tuple<string, string>> GeneratePagingSourceCodeAsync();
        protected abstract Task<TableDefinition> GetSchemaDefinitionAsync(string table);
    }
}
