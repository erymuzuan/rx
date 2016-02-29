using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ClrTypeVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            if (null == this.Type)
                throw new Exception("Cannot find type " + this.TypeName);
            
            return $"   public {Type.ToCSharp()} {Name} {{ get; set;}}";
        }

        public override string GeneratedCtorCode(WorkflowDefinition wd)
        {
            var type = Strings.GetType(this.TypeName);
            if (this.CanInitiateWithDefaultConstructor)
                return $" this.{Name} = new {type.FullName}();";

            return string.Empty;
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = $"[Variable] \"{this.Name}\" cannot contains space "
                });
            }
            if (null == this.Type)
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = $"[Variable]  cannot find the type \"{this.TypeName}\""
                });


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