using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public class Solution : Entity
    {
        [JsonIgnore]
        [ImportMany("SolutionCompiler", typeof(SolutionCompiler), AllowRecomposition = true)]
        public Lazy<SolutionCompiler, ISolutionCompilerMetadata>[] Compilers { get; set; }
        [JsonIgnore]
        [ImportMany("ProjectProvider", typeof(ProjectProvider), AllowRecomposition = true)]
        public Lazy<ProjectProvider, IProjectProviderMetadata>[] ProjectProviders { get; set; }

        public async Task<IEnumerable<WorkflowCompilerResult>> CompileAsync(string compilers)
        {
            var tasks = from c in this.Compilers.Where(x => compilers.Contains(x.Metadata.Name))
                let compiler = c.Value
                select compiler.CompileAsync(this);

            var results = await Task.WhenAll(tasks);

            return results;
        }

        public Task<IProjectProvider> LoadProjectAsync(ProjectMetadata pm)
        {
            var provider = this.ProjectProviders.SingleOrDefault(x => x.Metadata.Type == pm.Type);
            if(null == provider)
                throw new InvalidOperationException("Cannot find project provider for " + pm.Type);

            return provider.Value.LoadProjectAsync(pm);
        }

        private readonly ObjectCollection<ProjectMetadata> m_projectMetadataCollection = new ObjectCollection<ProjectMetadata>();

        public ObjectCollection<ProjectMetadata> ProjectMetadataCollection
        {
            get { return m_projectMetadataCollection; }
        }
        public static async Task<IEnumerable<JsRoute>> GetJsRoutes()
        {
            var ad = ObjectBuilder.GetObject<IDirectoryService>();
            var user = ad.CurrentUserName;

            var context = new SphDataContext();
            // ReSharper disable RedundantBoolCompare
            var screenActivityFormQuery = context.ScreenActivityForms.Where(e => e.IsPublished == true);
            var formQuery = context.EntityForms.Where(e => e.IsPublished == true);
            var viewQuery = context.EntityViews.Where(e => e.IsPublished == true);
            var edQuery = context.EntityDefinitions.Where(e => e.IsPublished == true);
            var rdlQuery = context.ReportDefinitions.Where(t => t.IsActive == true || (t.IsPrivate && t.CreatedBy == user));

            var rdlTask = context.LoadAsync(rdlQuery, includeTotalRows: true);
            var edTasks = context.LoadAsync(edQuery, includeTotalRows: true);
            var formTask = context.LoadAsync(formQuery, includeTotalRows: true);
            var screenActivityFormTask = context.LoadAsync(screenActivityFormQuery, includeTotalRows: true);
            var viewTask = context.LoadAsync(viewQuery, includeTotalRows: true);
            // ReSharper restore RedundantBoolCompare
            await Task.WhenAll(rdlTask, edTasks, formTask, viewTask);


            var rdlLo = await rdlTask;
            var edLo = await edTasks;
            var viewLo = await viewTask;
            var formsLo = await formTask;
            var screenFormsLo = await screenActivityFormTask;
            var routes = new List<JsRoute>();

            var reportDefinitions = new ObjectCollection<ReportDefinition>(rdlLo.ItemCollection);
            var entityDefinitions = new ObjectCollection<EntityDefinition>(edLo.ItemCollection);
            var views = new ObjectCollection<EntityView>(viewLo.ItemCollection);
            var forms = new ObjectCollection<EntityForm>(formsLo.ItemCollection);
            var screenActivityForms = new ObjectCollection<ScreenActivityForm>(screenFormsLo.ItemCollection);


            while (edLo.HasNextPage)
            {
                edLo = await context.LoadAsync(edQuery, edLo.CurrentPage + 1, includeTotalRows: true);
                entityDefinitions.AddRange(edLo.ItemCollection);
            }

            while (formsLo.HasNextPage)
            {
                formsLo = await context.LoadAsync(formQuery, formsLo.CurrentPage + 1, includeTotalRows: true);
                forms.AddRange(formsLo.ItemCollection);
            }
            while (screenFormsLo.HasNextPage)
            {
                screenFormsLo = await context.LoadAsync(screenActivityFormQuery, screenFormsLo.CurrentPage + 1, includeTotalRows: true);
                screenActivityForms.AddRange(screenFormsLo.ItemCollection);
            }
            while (viewLo.HasNextPage)
            {
                viewLo = await context.LoadAsync(viewQuery, viewLo.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(viewLo.ItemCollection);
            }

            while (rdlLo.HasNextPage)
            {
                rdlLo = await context.LoadAsync(rdlQuery, rdlLo.CurrentPage + 1, includeTotalRows: true);
                reportDefinitions.AddRange(rdlLo.ItemCollection);
            }


            // get valid users for ed
            var edDashboardUserTasks = from ed in entityDefinitions
                let p = ed.Performer
                where p.IsPublic
                      || (!string.IsNullOrWhiteSpace(p.UserProperty)
                          && !string.IsNullOrWhiteSpace(p.Value))
                select ed.Performer.GetUsersAsync(ed);
            var edDashboardUsers = await Task.WhenAll(edDashboardUserTasks);

            var screenActivityFormRoutes = from t in screenActivityForms
                select t.CreateJsRoute();
            var formRoutes = forms.Select(t => t.CreateJsRoute());
            var viewRoutes = from t in views
                select t.CreateJsRoute();

            var edRoutes = entityDefinitions
                .Where(t => t.Performer.Validate())
                .Select((t, i) => new
                {
                    Index = i,
                    Entity = t,
                    Permission = t.Performer,
                    Users = edDashboardUsers[i]
                })
                .Where(t => t.Permission.IsPublic || t.Users.Contains(user))
                .Select(t => t.Entity)
                .Select(t => new JsRoute
                {
                    Title = t.Plural,
                    Route = string.Format("{0}", t.Name.ToLowerInvariant()),
                    Caption = t.Plural,
                    Icon = t.IconClass,
                    ModuleId = string.Format("viewmodels/{0}", t.Name.ToLowerInvariant()),
                    Nav = t.IsShowOnNavigationBar
                });

            var rdlRoutes = reportDefinitions.Select(t => t.CreateJsRoute());

            // adapters
            var adapterDesginer = new Api.AdapterDesigner();
            var adapterRoutes = adapterDesginer.GetRoutes();
            routes.AddRange(adapterRoutes);

            routes.AddRange(viewRoutes);
            routes.AddRange(formRoutes);
            routes.AddRange(screenActivityFormRoutes);
            routes.AddRange(edRoutes);
            routes.AddRange(rdlRoutes);
            return routes;
        }
    }
}