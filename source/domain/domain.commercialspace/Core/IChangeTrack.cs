using System.Collections.Generic;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface IChangeTrack<T> 
    {
        IEnumerable<Change> GenerateChangeCollection(T p);

        string TrackingId { get; set; }
    }
}
