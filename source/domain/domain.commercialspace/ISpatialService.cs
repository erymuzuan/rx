using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Spatial;
using System.Threading.Tasks;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public interface ISpatialService<T> where T : SpatialEntity
    {
        Task UpdateAsync(T item);
        Task<string> GetEncodedPathAsync(Expression<Func<T, bool>> predicate);
        Task<string> GetWktAsync(Expression<Func<T, bool>> predicate);
        Task<Geography> GetGeographyAsync(Expression<Func<T, bool>> predicate);
        Task<LatLng> GetCenterAsync(Expression<Func<T, bool>> predicate);

        Task<IEnumerable<T>> ContainsAsync(Expression<Func<T, bool>> predicate, LatLng[] point);
        Task<IEnumerable<T>> GetNeighboursAsync(Expression<Func<T, bool>> predicate, double distance);

    }
}