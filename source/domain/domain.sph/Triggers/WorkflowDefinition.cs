using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public T GetActivity<T>(string webId) where T : Activity
        {
            return this.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }

        public async Task<Workflow> InitiateAsync(VariableValue[] values = null, ScreenActivity screen = null)
        {
            var typeName = string.Format("{3}.{0},workflows.{1}.{2}", this.WorkflowTypeName, this.Id, this.Version, this.CodeNamespace);
            var type = Type.GetType(typeName);
            if (null == type) throw new InvalidOperationException("Cannot instantiate  " + typeName);

            var initiator = this.GetInitiatorActivity();

            dynamic wf = Activator.CreateInstance(type);
            wf.Name = this.Name;
            wf.WorkflowDefinitionId = this.Id;
            wf.State = "Active";
            wf.WorkflowDefinition = this;

            // set the initial variable's value
            if (null != values)
                foreach (var vv in values)
                {
                    this.SetVariableValue(vv, wf, type);
                }


            if (null != screen)
            {
                wf.VariableValueCollection.ClearAndAddRange(values);
            }
            await wf.ExecuteAsync(initiator.WebId);
            return wf as Workflow;
        }

        private void SetVariableValue(VariableValue vv, Workflow wf, Type type)
        {
            var path = vv.Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            var prop = type.GetProperty(path[0]);
            if (path.Length == 1)
            {
                prop.SetValue(wf, vv.Value);
                return;
            }

            object dd = wf;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var pname = path[i];
                prop = dd.GetType().GetProperty(pname);
                dd = prop.GetValue(dd);
            }
            prop = dd.GetType().GetProperty(path.Last());
            prop.SetValue(dd, vv.Value);

        }

        public Activity GetInitiatorActivity()
        {
            return this.ActivityCollection.Single(p => p.IsInitiator);
        }

        public BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9 -]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must be started with letter.You cannot use symbol or number as first character" });

            if (this.ActivityCollection.Count(a => a.IsInitiator) != 1)
                result.Errors.Add(new BuildError(this.WebId) { Message = "You must have exactly one initiator activity" });

            if (string.IsNullOrWhiteSpace(this.SchemaStoreId))
                result.Errors.Add(new BuildError(this.WebId) { Message = "You must have exactly one schema defined" });


            foreach (var variable in this.VariableDefinitionCollection)
            {
                var v = variable.ValidateBuild(this);
                result.Errors.AddRange(v.Errors);
            }

            foreach (var activity in this.ActivityCollection)
            {
                var a = activity.ValidateBuild(this);
                if (null == a) continue;
                result.Errors.AddRange(a.Errors);
            }
            result.Result = !result.Errors.Any();
            return result;
        }


        public SphCompilerResult Compile(CompilerOptions options)
        {
            var project = (IProjectProvider)this;
            var projectDocuments = project.GenerateCode().ToList();
            var trees = (from c in projectDocuments
                         let x = c.GetCode()
                         let root = (CSharpSyntaxTree)CSharpSyntaxTree.ParseText(x)
                         select CSharpSyntaxTree.Create(root.GetRoot(),path:c.FileName)).ToList();

            var compilation = CSharpCompilation.Create(string.Format("{0}.wd.{1}", ConfigurationManager.ApplicationName, this.Id))
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

            return result;

        }



        [XmlIgnore]
        [JsonIgnore]
        public string WorkflowTypeName
        {

            get { return (this.Id.Humanize(LetterCasing.Title).Dehumanize() + "Workflow").Replace("WorkflowWorkflow", "Workflow"); }
        }
        [XmlIgnore]
        [JsonIgnore]
        public string CodeNamespace
        {
            get
            {
                var id = (this.Id.Humanize(LetterCasing.Title).Dehumanize());
                return string.Format("Bespoke.Sph.Workflows_{0}_{1}", id, this.Version);
            }
        }


    }
}