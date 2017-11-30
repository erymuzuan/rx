using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Domain.Extensions
{
    public static class ProjectDefinitionWithMembersExtension
    {
        public static string[] GetMembersPath(this IProjectDefinitionWithMembers project)
        {
            var list = new List<string>();
            list.AddRange(project.MemberCollection.Select(a => a.Name));
            foreach (var member in project.MemberCollection)
            {
                list.AddRange(member.GetMembersPath(""));
            }
            list.Add("Id");
            list.Add("WebId");
            list.Add("CreatedBy");
            list.Add("ChangedBy");
            list.Add("CreatedDate");
            list.Add("ChangedDate");
            return list.ToArray();
        }
    }
}
