using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class RouteParameterField : Field
    {
        public override async Task<IEnumerable<BuildError>> ValidateErrorsAsync(Filter filter)
        {
            var errors = (await base.ValidateErrorsAsync(filter)).ToList();
            Action<string> addError = message => errors.Add(new BuildError(filter.WebId, message));
            if (typeof(DateTime) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                DateTime date;
                if(!DateTime.TryParse(this.DefaultValue, out date))
                    addError($@"""{DefaultValue}"" is not a valid DateTime value for {Name} in {filter.Term} filter");
            }
            if (typeof(int) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                int val;
                if(!int.TryParse(this.DefaultValue, out val))
                    addError($@"""{DefaultValue}"" is not a valid Int32 value for {Name} in {filter.Term} filter");
            }
            if (typeof(decimal) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                decimal val;
                if(!decimal.TryParse(this.DefaultValue, out val))
                    addError($@"""{DefaultValue}"" is not a valid decimal value for {Name} in {filter.Term} filter");
            }
            if (typeof(bool) == this.Type && !string.IsNullOrWhiteSpace(this.DefaultValue))
            {
                bool val;
                if(!bool.TryParse(this.DefaultValue, out val))
                    addError($@"""{DefaultValue}"" is not a valid boolean value for {Name} in {filter.Term} filter, the only valid value is ""true"" or  ""false""");
            }
            return errors;
        }

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