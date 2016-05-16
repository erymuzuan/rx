using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    /// <summary>
    /// Just helper for the domain object collectionb
    /// </summary>
    public static class CollectionHelper
    {

        public static void Add(this IList<ValidationError> errors, ValidationError error)
        {
            errors.Add(error);
        }
        public static void Add(this IList<ValidationError> errors, string property, string message)
        {
            errors.Add(new ValidationError{PropertyName = property, Message = message});
        }
        public static void Add(this IList<ValidationError> errors, string property, string message, string location)
        {
            errors.Add(new ValidationError{PropertyName = property, Message = message, ErrorLocation = location});
        }
        /// <summary>
        /// Creates a new ObjectCollection from the source, this will also set the Bil Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static ObjectCollection<T> ToObjectCollection<T>(this IEnumerable<T> list) where T : DomainObject
        {
            var ls = new ObjectCollection<T>(list);
            ls.SetBill();

            return ls;
        }

        /// <summary>
        /// Remove the oldItem and insert new one at the oldItem index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The collection for which it's the container</param>
        /// <param name="oldItem">The old item to be replaced</param>
        /// <param name="newItem">The new item to be in place of the old one</param>
        public static void Replace<T>(this IList<T> list, T oldItem, T newItem)
        {
            var index = list.IndexOf(oldItem);
            list.Remove(oldItem);
            list.Insert(index, newItem);
        }


        public static ObjectCollection<T> Flatten<T>(this IEnumerable<IEnumerable<T>> list, bool setBil) where T : DomainObject
        {
            var olist = new ObjectCollection<T>();
            foreach (var ls in list)
            {
                olist.AddRange(ls);
            }
            if (setBil) olist.SetBill();

            return olist;
        }

        public static ObjectCollection<T> Flatten<T>(this IEnumerable<ObjectCollection<T>> list, bool setBil) where T : DomainObject
        {
            var olist = new ObjectCollection<T>();
            foreach (var ls in list)
            {
                olist.AddRange(ls);
            }
            if (setBil)
                olist.SetBill();

            return olist;
        }

        /// <summary>
        ///  For the domain object list, set the bil property in the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void SetBill<T>(this IEnumerable<T> list) where T : DomainObject
        {
            int bil = 0;
            foreach (var i in list)
            {
                bil++;
                i.Bil = bil;
            }
        }

        public static string[] ToArrayString<T>(this IEnumerable<T> list)
        {
            return list.Select(t => t.ToString()).ToArray();
        }

        public static string JoinString<T>(this IEnumerable<T> list, 
            string separator =",",
            Func<T, string> projection = null )
        {
            if (null == projection)
                projection = x => $"x";

            return string.Join(separator, list.Select(projection).ToArray());

        }

        public static IEnumerable<Change> GetChanges<T>(this IEnumerable<T> current, IEnumerable<T> changed, string fieldName)// where T : DomainObject
        {
            var currentList = current.ToList();
            var changedList = changed.ToList();
            var changeSet = new ObjectCollection<Change>();
            // for string
            if (typeof(T) == typeof(string))
            {
                var os = currentList.OrderBy(c => c).JoinString();
                var cs = changedList.OrderBy(c => c).JoinString();

                if (!string.Equals(os, cs, StringComparison.InvariantCultureIgnoreCase))
                {
                    changeSet.Add(new Change
                                      {
                                          OldValue = os,
                                          NewValue = cs,
                                          PropertyName = fieldName
                                      });
                }
            }

            foreach (var t0 in changedList.OfType<IChangeTrack<T>>().Where(t => string.IsNullOrWhiteSpace(t.TrackingId)))
            {
                throw new Exception("tracked item does not have tracking id " + t0);
            }
            foreach (var t9 in currentList.OfType<IChangeTrack<T>>().Where(t => string.IsNullOrWhiteSpace(t.TrackingId)))
            {
                throw new Exception("tracked item does not have tracking id " + t9);
            }

            // then compares each object
            foreach (var t in currentList.OfType<IChangeTrack<T>>())
            {
                if (null == t) continue;
                var t1 = t;
                var newItem = changedList.OfType<IChangeTrack<T>>().SingleOrDefault(f => f.TrackingId == t1.TrackingId);
                if (null != newItem)
                {
                    var itemChangeset = t.GenerateChangeCollection((T)newItem).ToList();
                    if (itemChangeset.Any())
                        changeSet.AddRange(itemChangeset);
                }

                if (null == newItem) // deleted
                {
                    var deletedChange = new Change
                                            {
                                                PropertyName = fieldName,
                                                NewValue = string.Empty,
                                                OldValue = t1.ToString(),
                                                Action = "Delete"

                                            };
                    changeSet.Add(deletedChange);

                }
            }
            // then compares each object for addition
            foreach (var t in changedList.OfType<IChangeTrack<T>>())
            {
                if (null == t) continue;
                var t1 = t;
                var old = currentList.OfType<IChangeTrack<T>>().SingleOrDefault(f => f.TrackingId == t1.TrackingId);


                if (null == old) // added
                {
                    var addedChange = new Change
                                          {
                                              PropertyName = fieldName,
                                              NewValue = t1.ToString(),
                                              OldValue = string.Empty,
                                              Action = "Add"

                                          };
                    changeSet.Add(addedChange);

                }
            }


            return changeSet;
        }
    }
}