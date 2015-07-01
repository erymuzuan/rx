using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Properties;
using Microsoft.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource]
    public partial class EntityDefinition : Entity
    {
        // reserved names
        private readonly string[] m_reservedNames = {"JavascriptTest",
                "Management",
                "Image",
                "Home",
                "BaseSph",
                "Map",
                "Admin",
                "ActivityScreen",
                "BaseApp",
                "Config",
                "Nav",
                "RoleSettings",
                "ScreenEditor",
                "TriggerSetup",
                "Users",
                "WorkflowDraft",
                typeof(ScreenActivity).Name,
                typeof(EntityDefinition).Name,
                typeof(AuditTrail).Name,
                typeof(BusinessRule).Name,
                typeof(BinaryStore).Name,
                typeof(SpatialEntity).Name,
                typeof(Entity).Name,
                typeof(Designation).Name,
                typeof(DocumentTemplate).Name,
                typeof(EmailAction).Name,
                typeof(EntityChart).Name,
                typeof(EntityDefinition).Name,
                typeof(EntityForm).Name,
                typeof(EntityView).Name,
                typeof(Message).Name,
                typeof(Organization).Name,
                typeof(Page).Name,
                typeof(ReportDefinition).Name,
                typeof(ReportDelivery).Name,
                typeof(SpatialStore).Name,
                typeof(Tracker).Name,
                typeof(Trigger).Name,
                typeof(UserProfile).Name,
                typeof(Watcher).Name,
                typeof(Workflow).Name,
                typeof(WorkflowDefinition).Name,
                typeof(EntityForm).Name,
                typeof(Message).Name};

        

        public override string ToString()
        {
            return this.Name;
        }

        public string[] GetMembersPath()
        {
            var list = new List<string>();
            list.AddRange(this.MemberCollection.Select(a => a.Name));
            foreach (var member in this.MemberCollection)
            {
                list.AddRange(member.GetMembersPath(""));
            }
            list.Add("Id");
            list.Add("WebId");
            list.Add("CreatedBy");
            list.Add("ChangedBy");
            list.Add("CreatedDate");
            list.Add("ChangedDate");
            return list.ToArray();
        }

        public BuildValidationResult CanSave()
        {
            var result = new BuildValidationResult();
            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });
            if (string.IsNullOrWhiteSpace(this.Name))
                result.Errors.Add(new BuildError(this.WebId, "Name is missing"));
            if (m_reservedNames.Select(a => a.Trim().ToLowerInvariant()).Contains(this.Name.Trim().ToLowerInvariant()))
                result.Errors.Add(new BuildError(this.WebId, $"The name [{this.Name}] is reserved for the system"));


            result.Result = !result.Errors.Any();
            return result;
        }

        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public async Task<BuildValidationResult> ValidateBuildAsync()
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(EntityDefinition)}.{nameof(BuildDiagnostics)}");

            var result = this.CanSave();
            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x);

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x);

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }

        public WorkflowCompilerResult Compile(CompilerOptions options, params string[] files)
        {
            if (files.Length == 0)
                throw new ArgumentException(Resources.Adapter_Compile_No_source_files_supplied_for_compilation, nameof(files));
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
                parameters.ReferencedAssemblies.Add(typeof(XmlAttributeAttribute).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(SmtpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(HttpClient).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(XElement).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(HttpResponseBase).Assembly.Location);
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


        private Member GetMember(string path)
        {
            if (!path.Contains("."))
                return this.MemberCollection.Single(a => a.Name == path);

            var paths = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var child = this.GetMember(paths.First());
            paths.RemoveAt(0);
            var nextPath = string.Join(".", paths);
            return this.GetMember(nextPath, child);
        }
        private Member GetMember(string path, Member member2)
        {
            if (!path.Contains("."))
                return member2.MemberCollection.Single(a => a.Name == path);

            var paths = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var child = this.GetMember(paths.First(), member2);
            var gg = path.Remove(0);
            var nextPath = string.Join(".", gg);
            return this.GetMember(nextPath, child);
        }
    }
}