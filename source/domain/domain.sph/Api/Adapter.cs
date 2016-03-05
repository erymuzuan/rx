using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Properties;

namespace Bespoke.Sph.Domain.Api
{
    [StoreAsSource(HasDerivedTypes = true)]
    public abstract partial class Adapter
    {
        public string[] SaveSources(IEnumerable<Class> classes, string folder)
        {
            var sources = classes.ToArray();
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources)
            {
                var file = Path.Combine(folder, cs.FileName);
                File.WriteAllText(file, cs.GetCode());
            }
            return sources.ToArray()
                    .Select(f => Path.Combine(folder, f.FileName))
                    .ToArray();
        }

        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var vr = new ObjectCollection<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Name))
                vr.Add("Name", "Name cannot be empty");

            return Task.FromResult(vr.AsEnumerable());
        }



        public WorkflowCompilerResult Compile(CompilerOptions options, params string[] files)
        {
            if (files.Length == 0)
                throw new ArgumentException(Resources.Adapter_Compile_No_source_files_supplied_for_compilation, nameof(files));
            foreach (var cs in files)
            {
                Debug.WriteLineIf(options.IsVerbose, cs);
            }

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"{ConfigurationManager.ApplicationName}.{this.Name}.dll"),
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
                parameters.ReferencedAssemblies.Add(typeof(MediaTypeFormatter).Assembly.Location);

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

        public virtual Task OpenAsync(bool verbose = false)
        {
            return Task.FromResult(0);
        }


        public async Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options = null)
        {
            if (null == options) options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Http.dll"));
            options.AddReference(typeof(System.Data.UpdateStatus));
            options.AddReference(typeof(System.Configuration.ConfigurationManager));

            var sourceFolder = $"{ConfigurationManager.GeneratedSourceDirectory}\\Adapter.{Name}";
            var sources = new List<string>();

            if (!Directory.Exists(sourceFolder))
                Directory.CreateDirectory(sourceFolder);

            foreach (var table in this.Tables)
            {
                var td = await this.GetSchemaDefinitionAsync(table.Name);
                td.CodeNamespace = this.CodeNamespace;
                var es = $"{sourceFolder}\\{table}.schema.json";

                if (!options.EmbeddedResourceCollection.Contains(es))
                {
                    File.WriteAllText(es, td.ToJsonString(true));
                    options.EmbeddedResourceCollection.Add(es);
                }

                var codes = td.GenerateCode(this);
                var tdSources = this.SaveSources(codes, sourceFolder);
                sources.AddRange(tdSources);

            }

            var adapterCodes = await this.GenerateSourceCodeAsync(options, this.CodeNamespace);
            var adapterSources = this.SaveSources(adapterCodes, sourceFolder);
            sources.AddRange(adapterSources);

            var odataTranslatorCode = await this.GenerateOdataTranslatorSourceCodeAsync();
            if (null != odataTranslatorCode)
            {
                var odataSource = this.SaveSources(new[] { odataTranslatorCode }, sourceFolder);
                sources.AddRange(odataSource);

            }

            var pagingCode = await this.GeneratePagingSourceCodeAsync();
            if (null != pagingCode)
            {
                var pagingSource = this.SaveSources(new[] { pagingCode }, sourceFolder);
                sources.AddRange(pagingSource);

            }


            var result = this.Compile(options, sources.ToArray());

            if (!result.Result)
                throw new Exception(string.Join("\r\n", result.Errors.Select(e => e.ToString())));

            return result;

        }

        protected abstract Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<Class> GenerateOdataTranslatorSourceCodeAsync();
        protected abstract Task<Class> GeneratePagingSourceCodeAsync();
        protected abstract Task<TableDefinition> GetSchemaDefinitionAsync(string table);

        public virtual void SaveAssets()
        {

        }


    }
}
