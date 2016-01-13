using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    [StoreAsSource(HasDerivedTypes = true)]
    public partial class PartialView : Entity
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
                throw new InvalidOperationException($"Fail to initialize MEF for PartialView.BuildDiagnostics");

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

        [XmlIgnore]
        [JsonIgnore]
        [ImportMany("FormRenderer", typeof(IFormRenderer), AllowRecomposition = true)]
        public Lazy<IFormRenderer, IFormRendererMetadata>[] FormRendererProviders { get; set; }

        public async Task<BuildValidationResult> RenderAsync(string name)
        {
            var build = new BuildValidationResult();
            if (null == this.FormRendererProviders)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.FormRendererProviders)
                throw new InvalidOperationException($"Fail to initialize MEF for {nameof(EntityForm)}.{nameof(FormRendererProviders)}");
            var provider = this.FormRendererProviders.SingleOrDefault(x => x.Metadata.Name == name);
            if (null == provider)
            {
                build.Errors.Add(new BuildError(this.WebId, "Cannot find renderer for " + name));
                return build;
            }

            var renderer = provider.Value;
            return await renderer.RenderAsync(this);
        }
    }
}