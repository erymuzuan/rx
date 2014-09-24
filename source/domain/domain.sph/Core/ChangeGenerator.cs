using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Bespoke.Sph.Domain
{
    public class ChangeGenerator
    {
        public IEnumerable<Change> GetChanges(object item1, object item2, string field = "")
        {
            var prepend = string.IsNullOrWhiteSpace(field) ? "" : field + ".";
            var changes = new ObjectCollection<Change>();
            var type = item1.GetType();
            if (type != item2.GetType())
                throw new ArgumentException("Must be the same type : " + type + " !=" + item2.GetType());

            var natives = new[] { typeof(int), typeof(DateTime), typeof(string), typeof(double), typeof(float), typeof(decimal), typeof(bool) };
            var properties = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).AsQueryable()
                             where natives.Contains(p.PropertyType)
                               && p.CanRead && p.CanWrite
                             select p;
            var nullableProperties = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).AsQueryable()
                                     where p.PropertyType.IsGenericType
                                     && natives.Contains(p.PropertyType.GenericTypeArguments[0])
                                       && p.CanRead && p.CanWrite
                                     select p;
          

            var colls = from p in type.GetProperties(BindingFlags.Instance | BindingFlags.Public).AsQueryable()
                        where p.Name.EndsWith("Collection")
                        select p;

            var customProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).AsQueryable()
                                       .Where(
                                           p =>
                                           (p.PropertyType.Namespace == typeof(Entity).Namespace || p.PropertyType.Namespace == type.Namespace )&&
                                           !p.PropertyType.Name.EndsWith("Collection")
                                           );

            foreach (var p in properties.Concat(nullableProperties))
            {
                var v1 = string.Format("{0}", p.GetValue(item1));
                var v2 = string.Format("{0}", p.GetValue(item2));
                if (p.PropertyType == typeof (decimal))
                {
                    var d1 = Convert.ToDecimal(v1);
                    var d2 = Convert.ToDecimal(v2);
                    if (d1 != d2)
                    {
                        changes.Add(new Change { PropertyName = prepend + p.Name, NewValue = v2, OldValue = v1, Action = "Changed" });
                    }
                    continue;
                }
                if (v1 != v2)
                {
                    changes.Add(new Change { PropertyName = prepend + p.Name, NewValue = v2, OldValue = v1, Action = "Changed" });
                }
            }

            foreach (var p in customProperties)
            {
                if (p.Name == "Item" && type.IsGenericType) continue;

                try
                {
                    var v1 = p.GetValue(item1);
                    var v2 = p.GetValue(item2);
                    var pchanges = this.GetChanges(v1, v2, prepend + p.Name);
                    changes.AddRange(pchanges);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Owner : {0}", type.Name);
                    Console.WriteLine("Property Name: {0}:{1}", p.Name, p.PropertyType);
                    Console.WriteLine("Exception : " + e.Message);
                }
            }

            foreach (var p in colls)
            {
                dynamic v1 = p.GetValue(item1);
                dynamic v2 = p.GetValue(item2);

                if (v2.Count - v1.Count != 0)
                {
                    changes.Add(new Change
                        {
                            Action = v2.Count - v1.Count > 0 ? "Added" : "Removed",
                            PropertyName = string.IsNullOrWhiteSpace(prepend + field) ? p.Name : field + "." + p.Name,
                            OldValue = string.Format("{0} item{1}", v1.Count, v1.Count == 1 ? "" : "s"),
                            NewValue = string.Format("{0} item{1}", v2.Count, v2.Count == 1 ? "" : "s")
                        });
                }


                bool isPrimitive = false;
                // look for change in the individual items, by what ? TrackingId on DomainObject is it's set
                foreach (dynamic t in v1)
                {
                    var t1 = t;
                    Type td = t1.GetType();
                    if (td.IsPrimitive || t1 is string)
                    {
                        isPrimitive = true;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(t.WebId)) continue;
                    dynamic t2 = null;
                    foreach (var o in v2)
                    {
                        if (o.WebId != t1.WebId) continue;
                        t2 = o;
                        break;
                    }
                    if (null != t2)
                    {
                        changes.AddRange(this.GetChanges(t1, t2, prepend + p.Name));
                    }
                }
                if (isPrimitive)
                {
                    var primitive1 = string.Join(",", v1);
                    var primitive2 = string.Join(",", v2);
                    if (string.Compare(primitive1, primitive2) != 0)
                    {
                        changes.Add(new Change{ Action = "Update",OldValue = primitive1, NewValue = primitive2,PropertyName = p.Name});
                    }
                }

            }

            return changes;
        }

    }
}