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
using Newtonsoft.Json;
using Polly;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    [StoreAsSource(HasDerivedTypes = true)]
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
            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(500),
                    (ex, ts) =>
                    {
                        ObjectBuilder.GetObject<ILogger>()
                        .Log(new LogEntry(ex));

                    });

            var errorTasks = this.BuildDiagnostics
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateErrorsAsync(this)))
                .Where(x => null != x)
                .Where(x => x.FinalException == null)
                .Select(x => x.Result)
                .ToArray();
            var errors = (await Task.WhenAll(errorTasks)).Where(x => null != x).SelectMany(x => x);

            var warningTasks = this.BuildDiagnostics
                .Select(d => policy.ExecuteAndCapture(() => d.ValidateWarningsAsync(this)))
                .Where(x => null != x)
                .Where(x => x.FinalException == null)
                .Select(x => x.Result)
                .ToArray();
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


            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"{ConfigurationManager.ApplicationName}.{this.Name}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true

                };

                parameters.AddReference(typeof(Entity),
                    typeof(int),
                    typeof(INotifyPropertyChanged),
                    typeof(Expression<>),
                    typeof(XmlAttributeAttribute),
                    typeof(SmtpClient),
                    typeof(HttpClient),
                    typeof(XElement),
                    typeof(HttpResponseBase),
                    typeof(ConfigurationManager));

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


        public Member GetMember(string path)
        {
            if (path == nameof(Id)) return new SimpleMember { Name = nameof(Id), Type = typeof(string) };
            if (path == nameof(CreatedDate)) return new SimpleMember { Name = nameof(CreatedDate), Type = typeof(DateTime) };
            if (path == nameof(ChangedDate)) return new SimpleMember { Name = nameof(ChangedDate), Type = typeof(DateTime) };
            if (path == nameof(CreatedBy)) return new SimpleMember { Name = nameof(CreatedBy), Type = typeof(string) };
            if (path == nameof(ChangedBy)) return new SimpleMember { Name = nameof(ChangedBy), Type = typeof(string) };

            if (!path.Contains("."))
            {

                var member = this.MemberCollection.SingleOrDefault(a => a.Name == path);
                if (null != member) return member;
                throw new InvalidOperationException($"Cannot find a member in {Name} with {path}");
            }

            var paths = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var child = this.GetMember(paths.First());
            paths.RemoveAt(0);
            var nextPath = string.Join(".", paths);
            return this.GetMember(nextPath, child);
        }
        private Member GetMember(string path, Member member2)
        {
            if (!path.Contains("."))
            {
                var vm = member2 as ValueObjectMember;
                if (null != vm)
                {
                    var members = vm.MemberCollection;
                    var rm1 = members.SingleOrDefault(a => a.Name == path);
                    if (null == rm1) throw new ArgumentException($"Cannot find any {path} in {Name}.{member2.Name}", nameof(path));
                    return rm1;
                }
                var rm = member2.MemberCollection.SingleOrDefault(a => a.Name == path);
                if (null == rm) throw new ArgumentException($"Cannot find any {path} in {Name}.{member2.Name}", nameof(path));
                return rm;
            }

            var paths = path.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var child = this.GetMember(paths.First(), member2);
            paths.RemoveAt(0);
            var nextPath = string.Join(".", paths);
            return this.GetMember(nextPath, child);
        }
    }
}