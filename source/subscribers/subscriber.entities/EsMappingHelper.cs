using System;
using System.Collections.Generic;
using System.Text;
using Bespoke.Sph.Domain;

namespace subscriber.entities
{
    public static class EsMappingHelper
    {
        public static string GetEsMappingType(this Member member)
        {
            var type = member.GetEsType();
            var map = new StringBuilder();
            map.AppendLine("{");
            if (member.Type == typeof(string))
            {
                map.AppendLinf("\"type\":\"string\",\"index\":\"{0}\"", (member.IsAnalyzed ? "analyzed" : "no"));
            }
            else
            {
                map.AppendLinf("    \"type\":\"{0}\"", type);
            }
            map.AppendLine("}");
            return map.ToString();
        }

        public static string GetEsType(this Member member)
        {
            if (typeof(string) == member.Type) return "string";
            if (typeof(int) == member.Type) return "integer";
            if (typeof(decimal) == member.Type) return "float";
            if (typeof(bool) == member.Type) return "boolean";
            if (typeof(DateTime) == member.Type) return "date";
            if (typeof(object) == member.Type) return null;
            if (typeof(Array) == member.Type) return null;
            return "";
        }

        public static string[] GetMemberMappings(this Member member, string parent = "")
        {
            var list = new List<string>();
            var type = member.GetEsType();
            var name = string.IsNullOrWhiteSpace(parent)
                ? member.Name
                : string.Format("{0}.{1}", parent, member.Name);
            if (!string.IsNullOrWhiteSpace(type))
            {
                var p = string.Format("         \"{0}\":{1}", name, member.GetEsMappingType());
                list.Add(p);
            }
            foreach (var m in member.MemberCollection)
            {
                list.AddRange(m.GetMemberMappings(name));
            }
            return list.ToArray();
        }

    }
}