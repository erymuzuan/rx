using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.SqlServer.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static SimpleMember AddMember(this EntityDefinition ed, string name, Type type, bool allowMultiples = false, bool nullable = false)
        {
            var sm = new SimpleMember
            {
                Name = name,
                Type = type,
                IsNullable = nullable,
                AllowMultiple = allowMultiples,
                WebId = $"/{ed.Name}.{name}"
            };
            ed.MemberCollection.Add(sm);
            return sm;
        }
    }
}
