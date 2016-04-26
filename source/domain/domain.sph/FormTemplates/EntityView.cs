using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class EntityView : Entity
    {
        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }
        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {

            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(EntityForm)}.{nameof(BuildDiagnostics)}");

            var result = new BuildValidationResult();
            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x);

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x);

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;

        }




        public string GenerateConditionalFormattingBinding()
        {
            if (!this.ConditionalFormattingCollection.Any())
                return string.Empty;
            var f = from s in this.ConditionalFormattingCollection
                    select $"'{s.CssClass}':{s.Condition}";
            return "css : {" + string.Join(",\r\n", f.ToArray()) + "}";
        }

     

        public override string ToString()
        {
            return $"[{this.Id}] {this.Name}";
        }

        public string GenerateRoute()
        {
            if (!this.RouteParameterCollection.Any())
                return $"{this.Route.ToLowerInvariant()}";
            return $"{this.Route.ToLowerInvariant()}"
                   + "/:" + string.Join("/:", this.RouteParameterCollection.Select(r => r.Name));
        }
    }
}