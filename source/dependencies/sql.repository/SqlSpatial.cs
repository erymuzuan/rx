using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Spatial;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlRepository
{
   public  class SqlSpatial<T> : ISpatialService<T> where T : SpatialEntity
   {
       public Task UpdateAsync(T item)
       {
           throw new NotImplementedException();
       }

       public Task<string> GetEncodedPathAsync(Expression<Func<T, bool>> predicate)
       {
           throw new NotImplementedException();
       }

       public Task<string> GetWktAsync(Expression<Func<T, bool>> predicate)
       {
           throw new NotImplementedException();
       }

       public Task<Geography> GetGeographyAsync(Expression<Func<T, bool>> predicate)
       {
           throw new NotImplementedException();
       }

       public Task<IEnumerable<T>> ContainsAsync(Expression<Func<T, bool>> predicate, LatLng point)
       {
           throw new NotImplementedException();
       }

       public Task<IEnumerable<T>> GetNeighboursAsync(Expression<Func<T, bool>> predicate, double distance)
       {
           throw new NotImplementedException();
       }
   }
}
