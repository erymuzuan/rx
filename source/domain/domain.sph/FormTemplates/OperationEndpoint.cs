using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [PersistenceOption(HasDerivedTypes = true, IsSource = true)]
    public partial class OperationEndpoint : Entity, IProjectDefinition
    {
        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {
            if (null == this.BuildDiagnostics)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.BuildDiagnostics)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(QueryEndpoint)}.BuildDiagnostics");

            var result = new BuildValidationResult();

            var errorTasks = this.BuildDiagnostics.Select(d => d.ValidateErrorsAsync(this, ed));
            var errors = (await Task.WhenAll(errorTasks)).SelectMany(x => x.ToArray());

            var warningTasks = this.BuildDiagnostics.Select(d => d.ValidateWarningsAsync(this, ed));
            var warnings = (await Task.WhenAll(warningTasks)).SelectMany(x => x.ToArray());

            result.Errors.AddRange(errors);
            result.Warnings.AddRange(warnings);


            result.Result = result.Errors.Count == 0;

            return result;
        }


        [ImportMany(typeof(IBuildDiagnostics))]
        [JsonIgnore]
        [XmlIgnore]
        public IBuildDiagnostics[] BuildDiagnostics { get; set; }

        public void AddRules(string rule)
        {
            this.Rules.Add(rule);
        }

        public Task<ValidationResult> ValidateAsync<T>(T item, EntityDefinition ed) where T : Entity
        {
            var rules = ed.BusinessRuleCollection.Where(x => this.Rules.Contains(x.Name));
            var result = item.ValidateBusinessRule(rules);

            return Task.FromResult(result);
        }
    }
}