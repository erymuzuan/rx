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
                var root = column.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).First();
                var prop = item.GetType().GetProperty(root);
                if (null == prop)
                {
                    Console.WriteLine("[.]Cannot get property for {0}", root);
                    return null;
                }
                var item2 = prop.GetValue(item) as DomainObject;


                var prop2 = column.Remove(0, root.Length + 1);
                return item2.MapColumnValue(prop2);
            }

            var prop3 = item.GetType().GetProperty(column);
            if (null == prop3)
            {
                Console.WriteLine("Cannot get property {0} for {1}", column, item.GetType().FullName);
                return null;
            }
            var val = prop3.GetValue(item);
            return val ?? DBNull.Value;
        }
    }
}