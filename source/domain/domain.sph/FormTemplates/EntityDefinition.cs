using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace Bespoke.Sph.Domain
{
    public partial class EntityDefinition : Entity
    {
        private void ValidateMember(Member member, BuildValidationResult result)
        {
            var forbiddenNames =
                typeof(Entity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(p => p.Name)
                    .ToList();
            forbiddenNames.AddRange(new[] { this.Name + "Id", "WebId", "CreatedDate", "CreatedBy", "ChangedBy", "ChangedDate" });

            const string pattern = "^[A-Za-z][A-Za-z0-9_]*$";
            var message = string.Format("[Member] \"{0}\" is not valid identifier", member.Name);
            var validName = new Regex(pattern);
            if (!validName.Match(member.Name).Success)
                result.Errors.Add(new BuildError(member.WebId) { Message = message });
            if (forbiddenNames.Contains(member.Name))
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " is a reserved name" });
            if (null == member.TypeName)
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " does not have a type" });

            foreach (var m in member.MemberCollection)
            {
                this.ValidateMember(m, result);
            }
        }

        public string[] GetMembersPath()
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => a.Name));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath(""));
            }
            return list.ToArray();
        }

        public BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9_]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            foreach (var member in this.MemberCollection)
            {
                this.ValidateMember(member, result);
            }

            var names = this.MemberCollection.Select(a => a.Name);
            var duplicates = names.GroupBy(a => a).Any(a => a.Count() > 1);
            if(duplicates)
                result.Errors.Add(new BuildError(this.WebId, "There are duplicates field names"));


            result.Result = !result.Errors.Any();
            return result;
        }

        public WorkflowCompilerResult Compile(CompilerOptions options)
        {
            var code = this.GenerateCode();
            Debug.WriteLineIf(options.IsVerbose, code);

            var sourceFile = string.Empty;
            if (!string.IsNullOrWhiteSpace(options.SourceCodeDirectory))
            {
                sourceFile = Path.Combine(options.SourceCodeDirectory,
                    string.Format("{0}.cs", this.Name));
                File.WriteAllText(sourceFile, code);
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

                foreach (var ass in options.ReferencedAssemblies)
                {
                    parameters.ReferencedAssemblies.Add(ass.Location);
                }
                var result = !string.IsNullOrWhiteSpace(sourceFile) ? provider.CompileAssemblyFromFile(parameters, sourceFile)
                    : provider.CompileAssemblyFromSource(parameters, code);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;
                cr.Errors.AddRange(this.GetCompileErrors(result, code));

                return cr;
            }
        }

        private IEnumerable<BuildError> GetCompileErrors(CompilerResults result, string code)
        {
            var temp = Path.GetTempFileName() + ".cs";
            File.WriteAllText(temp, code);
            var sources = File.ReadAllLines(temp);
            var list = (from object er in result.Errors.OfType<CompilerError>()
                        select this.GetSourceError(er as CompilerError, sources));
            File.Delete(temp);

            return list;
        }

        private BuildError GetSourceError(CompilerError er, string[] sources)
        {
            var member = string.Empty;
            for (var i = 0; i < er.Line; i++)
            {
                if (sources[i].StartsWith("//exec:"))
                    member = sources[i].Replace("//exec:", string.Empty);
            }
            var message = er.ErrorText;

            if (er.Line == 0)
                return new BuildError(member, message);

            return new BuildError(member, message)
            {
                Code = sources[er.Line - 1],
                Line = er.Line
            };

        }
    }
}