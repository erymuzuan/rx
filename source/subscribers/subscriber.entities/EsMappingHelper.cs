using System;
using System.Collections.Generic;
using System.Linq;
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

            var indexed = (member.IsNotIndexed ? "no" : "analyzed");

            map.Append("{");
            if (member.Type == typeof (string))
            {
                if (!member.IsNotIndexed)
                    indexed = (member.IsAnalyzed ? "analyzed" : "not_analyzed");
            }

            map.AppendFormat("\"type\":\"{0}\"", type);
            map.AppendFormat(",\"index\":\"{0}\"", indexed);
            map.AppendFormat(",\"boost\":{0}", Math.Max(1,member.Boost));
            map.AppendFormat(",\"include_in_all\":{0}", (!member.IsExcludeInAll).ToString().ToLowerInvariant());
            
            if ((new[] { typeof(int), typeof(decimal), typeof(DateTime) }).Contains(member.Type))
            {
                map.Append(",\"ignore_malformed\":false");
            }
            map.Append("}");
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
                var p = string.Format("             \"{0}\":{1}", name, member.GetEsMappingType());
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