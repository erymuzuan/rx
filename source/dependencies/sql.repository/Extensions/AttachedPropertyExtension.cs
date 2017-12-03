using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class AttachedPropertyExtension
    {
        public static AttachedProperty Add<T>(this IList<AttachedProperty> list, string name, string help, string description = "", T[] allowedValues = null)
        {
            var prop = new AttachedProperty
            {
                ProviderName = "SqlServer2016",
                Name = name,
                Type = typeof(T),
                Help = help,
                Description = string.IsNullOrWhiteSpace(description) ? help : description,
                AttachedTo = null,
                AllowedValue = (allowedValues ?? Array.Empty<T>()).OfType<object>().ToArray()
            };
            list.Add(prop);
            return prop;
        }
        public static AttachedProperty Add<T>(this IList<AttachedProperty> list, Member member, string name, string help, string description = "", T[] allowedValues = null)
        {
            var prop = new AttachedProperty
            {
                ProviderName = "SqlServer2016",
                Name = name,
                Type = typeof(T),
                Help = help,
                Description = string.IsNullOrWhiteSpace(description) ? help : description,
                AttachedTo = member.WebId,
                AllowedValue = (allowedValues ?? Array.Empty<T>()).OfType<object>().ToArray()
            };
            list.Add(prop);
            return prop;
        }
    }
}
