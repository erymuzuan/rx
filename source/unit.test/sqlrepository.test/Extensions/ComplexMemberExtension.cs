using System;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.SqlServer.Extensions
{
    public static class ComplexMemberExtension
    {
        public static SimpleMember AddMember(this ComplexMember mbr, string name, Type type, bool filterable, bool allowMultiples = false, bool nullable = false, bool excludeInAll = false, bool analyzed = false)
        {
            var sm = new SimpleMember
            {
                Name = name,
                Type = type,
                IsFilterable = filterable
                ,
                IsAnalyzed = analyzed,
                IsExcludeInAll = excludeInAll,
                IsNullable = nullable,
                AllowMultiple = allowMultiples
            };
            mbr.MemberCollection.Add(sm);
            return sm;
        }
    }
}