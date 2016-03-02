using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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

        public override async Task<string[]> GetMembersPathAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var list = new List<string>();
            var entity = await context.LoadOneAsync<EntityDefinition>(e => e.Name == this.Type.Name);
            if (null != entity)
            {
                list.AddRange(entity.GetMembersPath().Select(x => this.Name + "." + x));
            }
            else
            {
                var properties = this.Type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                list.AddRange(properties.Select(x => this.Name + "." + x.Name));
            }

            return list.ToArray();
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