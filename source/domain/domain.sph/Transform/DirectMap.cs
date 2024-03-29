using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class DirectMap : Map
    {
        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Source))
                errors.Add("Source", "Source cannot be empty");
            if (string.IsNullOrWhiteSpace(this.Destination))
                errors.Add("Destination", "Destination cannot be empty");
            return Task.FromResult(errors.AsEnumerable());
        }

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


        public override Task<string> ConvertAsync(object source)
        {
            var context = new RuleContext(source as Entity);
            var df = new DocumentField { Path = this.Source };
            var json = $"\"{this.Destination}\":\"{df.GetValue(context)}\"";
            return Task.FromResult(json);
        }

        public override string GenerateCode()
        {
            if (this.Source.Contains("-"))return string.Empty;
            if (this.Destination.Contains("-"))return string.Empty;

            return string.Format("dest.{1} = item.{0};", this.Source, this.Destination);
        }
    }
}