using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

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

        public SphCompilerResult Compile(CompilerOptions options, params string[] files)
        {
            var project = (IProjectProvider)this;
            var projectDocuments = project.GenerateCode().ToList();
            var trees = (from c in projectDocuments
                         let x = c.GetCode()
                         let root = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x)
                         select CSharpSyntaxTree.Create(root.GetRoot(), path: c.FileName)).ToList();

            var compilation = CSharpCompilation.Create(string.Format("{0}.{1}", ConfigurationManager.ApplicationName, this.Id))
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(project.References)
                .AddSyntaxTrees(trees);

            var errors = compilation.GetDiagnostics()
                .Where(d => d.Id != "CS8019")
                .Select(d => new BuildError(d));

            var result = new SphCompilerResult { Result = true };
            result.Errors.AddRange(errors);
            result.Result = result.Errors.Count == 0;
            if (DebuggerHelper.IsVerbose)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                result.Errors.ForEach(Console.WriteLine);
                Console.ForegroundColor = color;
            }
            if (!result.Result || !options.Emit)
                return result;

            if (null == options.Stream)
                throw new ArgumentException("To emit please provide a stream in your options", "options");

            var k = compilation.Emit(options.Stream);
            result.Result = k.Success;
            var errors2 = k.Diagnostics.Select(v => new BuildError(v));
            result.Errors.AddRange(errors2);

            return result;
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