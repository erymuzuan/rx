using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.SqlServer.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static SimpleMember AddMember(this EntityDefinition ed, string name, Type type, bool filterable, bool allowMultiples = false, bool nullable = false, bool excludeInAll = false, bool analyzed = false)
        {
            var sm = new SimpleMember
            {
                Name = name,
                Type = type,
                IsFilterable = filterable,
                IsAnalyzed = analyzed,
                IsExcludeInAll = excludeInAll,
                IsNullable = nullable,
                AllowMultiple = allowMultiples,
                WebId = $"/{ed.Name}.{name}"
            };
            ed.MemberCollection.Add(sm);
            return sm;
        }
    }
}
