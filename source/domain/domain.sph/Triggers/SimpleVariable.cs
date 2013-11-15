using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            return string.Format("public {0} {1}{{get;set;}}", this.Type.FullName, this.Name);
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError { Message = string.Format("[Variable] \"{0}\" cannot contains space ", this.Name) });
            }

            return result;
        }

        [XmlIgnore]
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Type.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
    }
}