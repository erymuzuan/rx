using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("ProjectProvider", typeof(ProjectProvider))]
    [ProjectProviderMetadata(Type = typeof(EntityDefinition))]
    class EntityDefinitionProjectProvider : ProjectProvider
    {
        public override async Task<IProjectProvider> LoadProjectAsync(ProjectMetadata pm)
        {
            var context = new SphDataContext();
            return await context.LoadOneAsync<EntityDefinition>(x => x.Name == pm.Name);
        }
    }
    [Export("ProjectProvider", typeof(ProjectProvider))]
    [ProjectProviderMetadata(Type = typeof(WorkflowDefinition))]
    class WorkflowDefinitionProjectProvider : ProjectProvider
    {
        public override async Task<IProjectProvider> LoadProjectAsync(ProjectMetadata pm)
        {
            var context = new SphDataContext();
            return await context.LoadOneAsync<WorkflowDefinition>(x => x.Name == pm.Name);
        }
    }
}