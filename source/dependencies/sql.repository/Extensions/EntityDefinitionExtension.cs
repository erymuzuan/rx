using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class EntityDefinitionExtension
    {

        public static IEnumerable<Member> GetFilterableMembers(this EntityDefinition ed, string parent = "", IList<Member> members = null)
        {
            members = members ?? ed.MemberCollection;
            var filterables = new ObjectCollection<Member>();
            var simples = members.OfType<SimpleMember>().Where(m => m.IsFilterable)
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.OfType<ComplexMember>()
                .Select(m => ed.GetFilterableMembers($"{parent}{m.Name}.", m.MemberCollection)).ToList()
                .SelectMany(m =>
                {
                    var enumerable = m as Member[] ?? m.ToArray();
                    return enumerable;
                })
                .ToList();
            filterables.AddRange(simples);
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName) || !m.FullName.EndsWith(m.Name))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;
        }

        public static string[] CreateIndexSql(this EntityDefinition item, int? version = 13)
        {
            var sql = from m in item.GetFilterableMembers()
                select m.CreateIndex(item, version);

            return sql.ToArray();
        }
    }
}