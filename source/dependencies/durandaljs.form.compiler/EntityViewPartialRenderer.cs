using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export]
    public class EntityViewPartialRenderer
    {
        public Task<string> GenerateCodeAsync(EntityView view, IProjectProvider project)
        {
            return Task.FromResult("partial");
        }
    }
}