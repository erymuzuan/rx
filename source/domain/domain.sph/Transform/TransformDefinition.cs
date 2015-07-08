using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CSharp;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(HasDerivedTypes = true)]
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

        public Task<WorkflowCompilerResult> CompileAsync(CompilerOptions options, params string[] files)
        {
            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);

            if (files.Length == 0)
                throw new ArgumentException("No source files supplied for compilation", nameof(files));
            foreach (var cs in files)
            {
                Debug.WriteLineIf(options.IsVerbose, cs);
            }

            using (var provider = new CSharpCodeProvider())
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
                parameters.ReferencedAssemblies.Add(typeof(System.Xml.Serialization.XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Mail.SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Net.Http.HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Xml.Linq.XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Web.HttpResponseBase).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(ConfigurationManager).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Data.SqlClient.SqlConnection).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Configuration.ConfigurationManager).Assembly.Location);
                foreach (var ra in this.ReferencedAssemblyCollection)
                {
                    parameters.ReferencedAssemblies.Add(ra.Location);
                }

                if (!string.IsNullOrWhiteSpace(this.InputTypeName))
                    parameters.ReferencedAssemblies.Add(this.InputType.Assembly.Location);
                else
                {
                    foreach (var p in this.InputCollection)
                    {
                        var type = Type.GetType(p.TypeName);
                        if (null != type)
                            parameters.ReferencedAssemblies.Add(type.Assembly.Location);
                    }
                }
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
                return Task.FromResult(cr);

            }
        }


        public override bool Validate()
        {
            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);
            if (string.IsNullOrWhiteSpace(this.Name))
                return false;
            return this.MapCollection.All(x => x.Validate());

        }


        public void AddFunctoids(params Functoid[] functoids)
        {
            this.FunctoidCollection.AddRange(functoids);
        }

        public async Task<BuildValidationResult> ValidateBuildAsync()
        {
            this.FunctoidCollection.ForEach(x => x.TransformDefinition = this);
            this.MapCollection.ForEach(x => x.TransformDefinition = this);

            var result = new BuildValidationResult();
            if (string.IsNullOrWhiteSpace(this.Name))
                result.Errors.Add(new BuildError { Message = "Name cannot be null or empty", ItemWebId = this.WebId });
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });

            var tasks = from m in this.MapCollection
                        select m.ValidateAsync();
            var maps = (await Task.WhenAll(tasks)).SelectMany(x => x.ToArray())
                .Select(x => new BuildError(x.ErrorLocation, x.Message));
            result.Errors.AddRange(maps);

            var fntTasks = from m in this.FunctoidCollection
                           select m.ValidateAsync();
            var functoidsValidation = (await Task.WhenAll(fntTasks)).SelectMany(x => x.ToArray())
                .Select(x => new BuildError(x.ErrorLocation, x.Message));

            result.Errors.AddRange(functoidsValidation);


            var distintcs = result.Errors.Distinct().ToArray();
            result.Errors.ClearAndAddRange(distintcs);


            result.Result = !result.Errors.Any();
            return result;


        }


        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            throw new Exception("Use ValidateBuildAsync");
        }
    }
}