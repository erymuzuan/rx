using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleVariable : Variable
    {
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

        public override string GetEmptyJson(WorkflowDefinition wd)
        {
            if (typeof (string) == this.Type)
            {
                return "''";
            }
            if (typeof (bool) == this.Type) return "false";

            return base.GetEmptyJson(wd);
        }
    }
}