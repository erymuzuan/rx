using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class RouteParameterField : Field
    {
        public override async Task<IEnumerable<BuildDiagnostic>> ValidateErrorsAsync(Filter filter)
        {
            var errors = (await base.ValidateErrorsAsync(filter)).ToList();
            void AddError(string message) => errors.Add(new BuildDiagnostic(filter.WebId, message));
            if (typeof(DateTime) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                if (!DateTime.TryParse(this.DefaultValue, out DateTime _))
                    AddError($@"""{DefaultValue}"" is not a valid DateTime value for {Name} in {filter.Term} filter");
            }
            if (typeof(int) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                if (!int.TryParse(this.DefaultValue, out int _))
                    AddError($@"""{DefaultValue}"" is not a valid Int32 value for {Name} in {filter.Term} filter");
            }
            if (typeof(decimal) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                if (!decimal.TryParse(this.DefaultValue, out decimal _))
                    AddError($@"""{DefaultValue}"" is not a valid decimal value for {Name} in {filter.Term} filter");
            }
            if (typeof(bool) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                if (!bool.TryParse(this.DefaultValue, out bool _))
                    AddError($@"""{DefaultValue}"" is not a valid boolean value for {Name} in {filter.Term} filter, the only valid value is ""true"" or  ""false""");
            }
            return errors;
        }

        public string GetRouteConstraint()
        {
            var constraint = string.IsNullOrWhiteSpace(this.Constraints) ? "" : ":" + this.Constraints;
            var type = this.Type;
            if (type == typeof(string)) return string.Empty + constraint;
            if (type == typeof(short)) return ":int" + constraint;
            if (type == typeof(byte)) return ":int" + constraint;
            if (type == typeof(Guid)) return ":guid";
            if (type == typeof(DateTime)) return ":datetime";
            return ":" + type.ToCSharp();
        }


        public override object GetValue(RuleContext context)
        {
            return $"<<{this.Name}>>";
        }
        [JsonIgnore]
        public virtual Type Type
        {
            get => string.IsNullOrWhiteSpace(this.TypeName) ?
                null :
                Strings.GetType(this.TypeName);
            set => this.TypeName = value.GetShortAssemblyQualifiedName();
        }

        public string GenerateParameterCode()
        {
            var type = Type.ToCSharp();
            if (!this.IsOptional)
                return $@"{type} {Name}";

            if (this.Type == typeof(string))
                return $@"{type} {Name} = ""{DefaultValue}""";
            if (this.Type == typeof(DateTime))
                return $@"DateTime? {Name}DateTime = null";
            return $@"{type} {Name} = {DefaultValue}";
        }
        public string GenerateDefaultValueCode()
        {
            if (!this.IsOptional) return null;
            if (string.IsNullOrWhiteSpace(this.DefaultValue))
                return null;


            if (this.Type == typeof(DateTime))
                return $@"var {Name} = {Name}DateTime ?? DateTime.Parse(""{DefaultValue}"");";
            return null;
        }

    }
}