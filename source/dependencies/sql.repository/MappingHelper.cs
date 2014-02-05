using System;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public static class MappingHelper
    {
        public static object MapColumnValue(this DomainObject item, string column)
        {

            if (column.Contains("."))
            {
                var prop = column.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First();
                var propInfo = item.GetType().GetProperty(prop);
                if (null == propInfo) throw new InvalidOperationException("[.]Cannot get property " + prop);
                var item2 = propInfo.GetValue(item) as DomainObject;
               

                var prop2 = column.Remove(0, prop.Length+1);
                return item2.MapColumnValue(prop2);
            }

            var propInfo3 = item.GetType().GetProperty(column);
            if (null == propInfo3) throw new InvalidOperationException("Cannot get property " + column + " for " + item.GetType().FullName);
            var val = propInfo3.GetValue(item);
            if (null != val) return val;

            return DBNull.Value;
        }
    }
}