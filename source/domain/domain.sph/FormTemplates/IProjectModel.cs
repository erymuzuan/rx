using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public interface IProjectModel
    {
        IEnumerable<Member> Members { get; }
        string Name { get; }
        string DefaultNamespace { get;  }
        string Id { get;  }
    }
}