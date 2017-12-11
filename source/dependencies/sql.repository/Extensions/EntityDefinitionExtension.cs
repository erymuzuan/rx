using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.SqlRepository.Extensions
{
    public static class EntityDefinitionExtension
    {

        public static IEnumerable<Member> GetFilterableMembers(this EntityDefinition ed, IProjectBuilder compiler, string parent = "", IList<Member> members = null)
        {

            members = members ?? ed.MemberCollection;

            var repos = ObjectBuilder.GetObject<ISourceRepository>();

            bool IsFilterable(SimpleMember member)
            {
                var properties = repos.GetAttachedPropertiesAsync(compiler, ed, member).Result.ToList();
                var prop = properties.SingleOrDefault(x => x.Name == "SqlIndex" && x.AttachedTo == member.WebId);
                if (null == prop) return false;
                return prop.GetValue(false);
            }

            var filterables = new ObjectCollection<Member>();
            var simples = members.OfType<SimpleMember>().Where(IsFilterable)
                .Where(m => m.Type != typeof(object))
                .Where(m => m.Type != typeof(Array))
                .ToList();
            var list = members.OfType<ComplexMember>()
                .Select(m => ed.GetFilterableMembers(compiler, $"{parent}{m.Name}.", m.MemberCollection)).ToList()
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

        public static string[] CreateIndexSql(this EntityDefinition item, IProjectBuilder compiler, int? version = 13)
        {
            var sql = from m in item.GetFilterableMembers(compiler)
                      select m.CreateIndex(item, null, version);

            return sql.ToArray();
        }
    }
}