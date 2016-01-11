using System;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class SimpleMember
    {
        private string GetEsType()
        {
            var member = this;
            if (typeof(string) == member.Type) return "string";
            if (typeof(int) == member.Type) return "integer";
            if (typeof(decimal) == member.Type) return "float";
            if (typeof(bool) == member.Type) return "boolean";
            if (typeof(DateTime) == member.Type) return "date";
            if (typeof(object) == member.Type) return "object";
            if (typeof(Array) == member.Type) return "object";
            return "";
        }

        private string GetEsMappingType()
        {
            var type = this.GetEsType();
            var map = new StringBuilder();

            var indexed = (this.IsNotIndexed ? "no" : "analyzed");

            map.Append("{");
            if (this.Type == typeof(string))
            {
                if (!this.IsNotIndexed)
                    indexed = (this.IsAnalyzed ? "analyzed" : "not_analyzed");
            }

            if (this.Type == typeof(bool))
                indexed = "not_analyzed";

            var boost = this.Boost;
            if (indexed == "not_analyzed")
                boost = 1;

            var includeAll = (!IsExcludeInAll).ToString().ToLowerInvariant();
            map.Append($"\"type\":\"{type}\"");
            map.Append($",\"index\":\"{indexed}\"");
            map.Append($",\"boost\":{Math.Max(1, boost)}");
            map.Append($",\"include_in_all\":{includeAll}");

            if ((new[] { typeof(int), typeof(decimal), typeof(DateTime) }).Contains(Type))
            {
                map.Append(",\"ignore_malformed\":false");
            }
            map.Append("}");
            return map.ToString();
        }

        private string GetObjectMapping()
        {

            var map = new StringBuilder();
            map.AppendLine($"    \"{Name}\":{{");

            map.AppendLine("        \"type\":  \"object\",");
            map.AppendLine("        \"properties\":{");

            var memberMappings = string.Join(",\r\n", this.MemberCollection.Select(d => d.GetEsMapping()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }

        public override string GetEsMapping()
        {
            return $"             \"{Name}\":{GetEsMappingType()}";
        }
    }
}