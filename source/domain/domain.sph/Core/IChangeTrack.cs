using System.Collections.Generic;

namespace Bespoke.Sph.Domain
{
    public interface IChangeTrack<in T> 
    {
        IEnumerable<Change> GenerateChangeCollection(T p);
        string TrackingId { get;}
    }
}
