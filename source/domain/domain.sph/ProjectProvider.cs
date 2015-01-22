using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public abstract class  ProjectProvider
    {
        public abstract Task<IProjectProvider> LoadProjectAsync(ProjectMetadata pm);
    }
}