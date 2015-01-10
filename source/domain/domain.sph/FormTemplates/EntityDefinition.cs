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
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Properties;
using Microsoft.CSharp;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Name = {Name}")]
    public partial class EntityDefinition : Entity
    {
        // reserved names
        private readonly string[] m_reservedNames = new[] {"JavascriptTest", 
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
                typeof(ScreenActivityForm).Name, 
                typeof(ReportDefinition).Name, 
                typeof(ReportDelivery).Name, 
                typeof(SpatialStore).Name, 
                typeof(Tracker).Name, 
                typeof(Trigger).Name, 
                typeof(UserProfile).Name, 
                typeof(Watcher).Name, 
                typeof(Workflow).Name, 
                typeof(WorkflowDefinition).Name, 
                typeof(EntityForm).Name, typeof(Message).Name};


        private void ValidateMember(Member member, BuildValidationResult result)
        {
            var forbiddenNames =
                typeof(Entity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Select(p => p.Name)
                    .ToList();
            forbiddenNames.AddRange(new[] { this.Name + "Id", "WebId", "CreatedDate", "CreatedBy", "ChangedBy", "ChangedDate" });

            const string PATTERN = "^[A-Za-z][A-Za-z0-9_]*$";
            var message = string.Format("[Member] \"{0}\" is not valid identifier", member.Name);
            var validName = new Regex(PATTERN);
            if (!validName.Match(member.Name).Success)
                result.Errors.Add(new BuildError(member.WebId) { Message = message });
            if (forbiddenNames.Contains(member.Name))
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " is a reserved name" });
            if (null == member.TypeName)
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " does not have a type" });

            if (member.Type == typeof(Array) && !member.Name.EndsWith("Collection"))
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " must be appennd with \"Collection\"" });
            if (member.Type == typeof(object) && member.Name.EndsWith("Collection"))
                result.Errors.Add(new BuildError(member.WebId) { Message = "[Member] " + member.Name + " must not end with \"Collection\"" });


            foreach (var m in member.MemberCollection)
            {
                this.ValidateMember(m, result);
            }
        }

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
                result.Errors.Add(new BuildError(this.WebId, string.Format("The name [{0}] is reserved for the system", this.Name)));


            result.Result = !result.Errors.Any();
            return result;
        }

        public async Task<BuildValidationResult> ValidateBuildAsync()
        {
            var result = this.CanSave();
            var context = new SphDataContext();

            foreach (var member in this.MemberCollection)
            {
                this.ValidateMember(member, result);
            }

            var names = this.MemberCollection.Select(a => a.Name);
            var duplicates = names.GroupBy(a => a).Any(a => a.Count() > 1);
            if (duplicates)
                result.Errors.Add(new BuildError(this.WebId, "There are duplicates field names"));

            if (string.IsNullOrWhiteSpace(this.RecordName))
                result.Errors.Add(new BuildError(this.WebId, "Record name is missing"));
            if (string.IsNullOrWhiteSpace(this.Plural))
                result.Errors.Add(new BuildError(this.WebId, "Plural is missing"));

            if (this.MemberCollection.All(m => m.Name != this.RecordName))
                result.Errors.Add(new BuildError(this.WebId, "Record name is not registered in your schema as a first level member"));

            if (!this.Performer.Validate())
                result.Errors.Add(new BuildError(this.WebId, "You have not set the permission correctly"));

            // ReSharper disable RedundantBoolCompare
            var defaultForm = await context.LoadOneAsync<EntityForm>(f => f.IsDefault == true && f.EntityDefinitionId == this.Id);
            // ReSharper restore RedundantBoolCompare
            if (null == defaultForm)
                result.Errors.Add(new BuildError(this.WebId, "Please set a default form"));

            foreach (var operation in this.EntityOperationCollection)
            {
                var errors = (await operation.ValidateBuildAsync(this)).ToList();
                if (errors.Any())
                    result.Errors.AddRange(errors);
            }
            
            result.Result = result.Errors.Count == 0;
            return result;
        }

        public WorkflowCompilerResult Compile(CompilerOptions options, params string[] files)
        {
            if (files.Length == 0)
                throw new ArgumentException(Resources.Adapter_Compile_No_source_files_supplied_for_compilation, "files");
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