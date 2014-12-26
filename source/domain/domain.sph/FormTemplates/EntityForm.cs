using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class EntityForm : Entity
    {
        public async Task<BuildValidationResult> ValidateBuildAsync(EntityDefinition ed)
        {

            var result = new BuildValidationResult();
            var errors = from f in this.FormDesign.FormElementCollection
                         where f.IsPathIsRequired
                             && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")
                         select new BuildError
                         (
                             this.WebId,
                             string.Format("[Input] : {0} => '{1}' does not have path", this.Name, f.Label)
                         );
            var elements = from f in this.FormDesign.FormElementCollection
                           let err = f.ValidateBuild(ed)
                           where null != err
                           select err;

            var context = new SphDataContext();

            var formRouteCountTask = context.GetCountAsync<EntityForm>(f => f.Route == this.Route && f.Id != this.Id);
            var viewRouteCountTask = context.GetCountAsync<EntityView>(f => f.Route == this.Route);
            var entityRouteCountTask = context.GetCountAsync<EntityDefinition>(f => f.Name == this.Route);

            await Task.WhenAll(formRouteCountTask, viewRouteCountTask, entityRouteCountTask).ConfigureAwait(false);

            if (await formRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used by another form"));

            if (await viewRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used by a view"));

            if (await entityRouteCountTask > 0)
                result.Errors.Add(new BuildError(this.WebId, "The route is already in used, cannot be the same as an entity name"));

            var validName = new Regex(@"^[A-Za-z][A-Za-z0-9 -]*$");
            if (!validName.Match(this.Name).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Name must start with letter.You cannot use symbol or number as first character" });

            var validRoute = new Regex(@"^[a-z0-9-._]*$");
            if (!validRoute.Match(this.Route).Success)
                result.Errors.Add(new BuildError(this.WebId) { Message = "Route must be lower case.You cannot use symbol or number as first character, or other chars except _ - ." });

            if (this.CompilerCollection.Count == 0)
                result.Errors.Add(new BuildError(this.WebId) { Message = "You need to specify at least one compiler for the form." });


            result.Errors.AddRange(errors);
            result.Errors.AddRange(elements.SelectMany(v => v));
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