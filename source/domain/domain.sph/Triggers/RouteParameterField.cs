using System;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class RouteParameterField : Field
    {
        public override object GetValue(RuleContext context)
        {
            return $"<<{this.Name}>>";
        }
        [JsonIgnore]
        public virtual Type Type
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.TypeName) ?
                    null :
                    Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }

        public string GenerateParameterCode()
        {
            var type = Type.ToCSharp();
            if (string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                return $@"{type} {Name}";
            }
            if (this.Type == typeof(string))
                return $@"{type} {Name} = ""{DefaultValue}""";
            if (this.Type == typeof(DateTime))
                return $@"DateTime? {Name}DateTime = null";
            return $@"{type} {Name} = {DefaultValue}";
        }
        public string GenerateDefaultValueCode()
        {
            var type = Type.ToCSharp();
            if (string.IsNullOrWhiteSpace(this.DefaultValue))
                return null;


            if (this.Type == typeof(DateTime))
                return $@"var {Name} = {Name}DateTime ?? DateTime.Parse(""{DefaultValue}"");";
            return null;
        }

    }
}