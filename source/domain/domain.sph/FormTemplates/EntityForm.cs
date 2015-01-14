using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class EntityForm : Form
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

            // validates the element expression
            ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.Compilers)
                throw new InvalidOperationException("Failed to initialize compilers list with MEF");
            foreach (var cn in this.CompilerCollection)
            {
                var compiler = this.Compilers.FirstOrDefault(x => x.Metadata.Name == cn);
                if (null == compiler)
                {
                    result.Errors.Add(new BuildError(this.WebId) { Message = "Cannot find the compiler for " + cn });
                    continue;
                }
                foreach (var fe in this.FormDesign.FormElementCollection)
                {
                    var fe1 = fe;
                    var expressions = fe.CodeExpressions();
                    foreach (var d in expressions)
                    {
                        var diagnostics = from g in compiler.Value.GetDiagnostics(fe1, d, ed)
                                          let m = string.Format("[{0}] {1}", fe1.ElementId, g)
                                          select new BuildError(fe1.ElementId, m);
                        result.Errors.AddRange(diagnostics);
                    }
                }
            }
            var duplicateElementId = this.FormDesign.FormElementCollection.GroupBy(x => x.ElementId)
                .Where(x => x.Count() > 1)
                .Select(x => new BuildError(x.Key, string.Format("There {1} elements with the same id : {0}", x.Key, x.Count())));
            result.Errors.AddRange(duplicateElementId);

            result.Errors.AddRange(errors);
            result.Errors.AddRange(elements.SelectMany(v => v));
            result.Result = result.Errors.Count == 0;

            return result;
        }

        public async Task<BuildValidationResult> RenderAsync(string name)
        {
            var build = new BuildValidationResult();
            if (null == this.FormRendererProviders)
                ObjectBuilder.ComposeMefCatalog(this);
            if (null == this.FormRendererProviders) throw new InvalidOperationException("Cannot instantiate MEF");

            var provider = this.FormRendererProviders.SingleOrDefault(x => x.Metadata.Name == name);
            if (null == provider)
            {
                build.Errors.Add(new BuildError(this.WebId, "Cannot find renderer for " + name));
                return build;
            }

            var renderer = provider.Value;
            return await renderer.RenderAsync(this);
        }

        public JsRoute CreateJsRoute()
        {
            var t = this;
            return new JsRoute
            {
                Title = t.Name,
                Route = string.Format("{0}/:id", t.Route.ToLowerInvariant()),
                Caption = t.Name,
                Icon = t.IconClass,
                ModuleId = string.Format("viewmodels/{0}", t.Route.ToLowerInvariant()),
                Nav = false
            };
        }

        public async override Task<IProjectProvider> LoadProjectAsync()
        {
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(x => x.Id == this.EntityDefinitionId).ConfigureAwait(false);
            return ed;
        }
    }
}