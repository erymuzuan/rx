using System;
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
            if (member.Type == typeof(string))
            {
                if (!member.IsNotIndexed)
                    indexed = (member.IsAnalyzed ? "analyzed" : "not_analyzed");
            }

            if (member.Type == typeof(bool))
                indexed = "not_analyzed";

            var boost = member.Boost;
            if (indexed == "not_analyzed")
                boost = 1;

            map.AppendFormat("\"type\":\"{0}\"", type);
            map.AppendFormat(",\"index\":\"{0}\"", indexed);
            map.AppendFormat(",\"boost\":{0}", Math.Max(1, boost));
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
            if (typeof(object) == member.Type) return "object";
            if (typeof(Array) == member.Type) return "object";
            return "";
        }


        public static string GetObjectMapping(this Member member)
        {
            var map = new StringBuilder();
            map.AppendLinf("    \"{0}\":{{", member.Name);

            map.AppendLine("        \"type\":  \"object\",");
            map.AppendLine("        \"properties\":{");

            var memberMappings = string.Join(",\r\n", member.MemberCollection.Select(d => d.GetMemberMappings()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }

        public static string GetMemberMappings(this Member member)
        {

            if (member.Type == typeof(object) || member.Type == typeof(Array))
            {
                return member.GetObjectMapping();
            }
            var p = string.Format("             \"{0}\":{1}", member.Name, member.GetEsMappingType());
            return p;
        }

    }
}