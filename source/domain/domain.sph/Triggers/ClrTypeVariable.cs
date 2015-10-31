﻿using System;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ClrTypeVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            var code = new StringBuilder();
            if(null == this.Type)
                throw new Exception("Cannot find type " + this.TypeName);

            code.AppendLinf(this.CanInitiateWithDefaultConstructor ?
                "   private {0} m_{1} = new {0}();" :
                "   private {0} m_{1};", this.Type.FullName, this.Name);
            code.AppendLinf("   public {0} {1}", this.Type.FullName, this.Name);
            code.AppendLine("   {");
            code.AppendLinf("       get{{ return m_{0};}}", this.Name);
            code.AppendLinf("       set{{ m_{0} = value;}}", this.Name);
            code.AppendLine("   }");
            return code.ToString();
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId) { Message =$"[Variable] \"{this.Name}\" cannot contains space "
                });
            }
            if (null == this.Type)
                result.Errors.Add(new BuildError(this.WebId) { Message =$"[Variable]  cannot find the type \"{this.TypeName}\""
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