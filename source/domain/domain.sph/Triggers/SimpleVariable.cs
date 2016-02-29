using System;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            return $"public {this.Type.FullName} {this.Name}{{get;set;}}";
        }

        public override string GeneratedCtorCode(WorkflowDefinition wd)
        {
            var variable = this;
            var ctor = new StringBuilder();
            if (variable.Type == typeof(string))
                ctor.AppendLinf("           this.{0} = \"{1}\";", variable.Name, variable.DefaultValue);
            if (variable.Type == typeof(int))
                ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
            if (variable.Type == typeof(decimal))
                ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
            if (variable.Type == typeof(bool))
                ctor.AppendLinf("           this.{0} = {1};", variable.Name, variable.DefaultValue);
            if (variable.Type == typeof(DateTime))
                ctor.AppendLinf("           this.{0} = DateTime.Parse(\"{1}\");", variable.Name, variable.DefaultValue);

            return ctor.ToString();
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId) { Message = string.Format("[Variable] \"{0}\" cannot contains space ", this.Name) });
            }

            return result;
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
    }
}