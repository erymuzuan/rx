using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Humanizer;

using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : CustomProject
    {
        public T GetActivity<T>(string webId) where T : Activity
        {
            return this.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }

        public async Task<Workflow> InitiateAsync(VariableValue[] values = null, ScreenActivity screen = null)
        {
            var typeName = string.Format("{3}.{0},workflows.{1}.{2}", this.WorkflowTypeName, this.Id, this.Version, this.DefaultNamespace);
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


        [XmlIgnore]
        [JsonIgnore]
        public string WorkflowTypeName
        {

            get { return (this.Id.Humanize(LetterCasing.Title).Dehumanize() + "Workflow").Replace("WorkflowWorkflow", "Workflow"); }
        }
   


    }
}