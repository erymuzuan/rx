using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    /// <summary>
    /// Represent an interface for which a project could construct its class with Members
    /// </summary>
    public interface IProjectModel
    {
        IEnumerable<Member> GetMembers();
        string Name { get; }
        string DefaultNamespace { get;  }
        string Id { get;  }
    }
}