using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{
    public partial class ReceiveLocation : DomainObject
    {
        public virtual Task<IEnumerable<Class>> GenerateClassesAsync(ReceivePort port)
        {
            throw new System.NotImplementedException();
        }
    }
}