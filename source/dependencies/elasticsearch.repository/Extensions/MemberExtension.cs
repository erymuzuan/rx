using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class MemberExtension
    {
        public static string GetEsMapping(this Member member)
        {
            switch (member)
            {
                case SimpleMember sm:
                    return GetEsMapping(sm);
                case ComplexMember cm:
                    return GetEsMapping(cm);
                case ValueObjectMember vm:
                    return GetEsMapping(vm);
            }
            return null;
        }

        public static string GetEsMapping(this ComplexMember member)
        {
            var map = new StringBuilder();
            map.AppendLine($@"    ""{member.Name}"":{{");

            map.AppendLine(@"        ""type"":  ""object"",");
            map.AppendLine(@"        ""properties"":{");

            var memberMappings = member.MemberCollection.ToString(",\r\n", x => x.GetEsMapping());
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }



        private static string GetEsType(this SimpleMember sm)
        {

            if (typeof(string) == sm.Type) return "string";
            /* ES 5
            if (typeof(string) == sm.Type && sm.IsAnalyzed) return "text";
            if (typeof(string) == sm.Type && !sm.IsAnalyzed) return "keyword";
            */
            if (typeof(int) == sm.Type) return "integer";
            if (typeof(decimal) == sm.Type) return "float";
            if (typeof(bool) == sm.Type) return "boolean";
            if (typeof(DateTime) == sm.Type) return "date";
            if (typeof(object) == sm.Type) return "object";
            if (typeof(Array) == sm.Type) return "object";
            return "";
        }

        private static string GetEsMappingType(this SimpleMember sm)
        {
            var type = sm.GetEsType();
            var map = new StringBuilder();

            var indexed = "no";//TODO : (sm.IsNotIndexed ? "no" : "analyzed");

            map.Append("{");
            if (sm.Type == typeof(string))
            {
              //TODO :  if (!sm.IsNotIndexed)
              //      indexed = (sm.IsAnalyzed ? "analyzed" : "not_analyzed");
            }

           //TODO : if (sm.Type == typeof(bool))
           //     indexed = "not_analyzed";

            var boost = 5;//TODO sm.Boost;
            if (indexed == "not_analyzed")
                boost = 1;

            var includeAll = (/*TODO :!sm.IsExcludeInAll*/ true).ToString().ToLowerInvariant();
            map.Append($@"""type"":""{type}""");
            map.Append($@",""index"":""{indexed}""");
            map.Append($@",""boost"":{Math.Max(1, boost)}");
            map.Append($@",""include_in_all"":{includeAll}");

            if ((new[] { typeof(int), typeof(decimal), typeof(DateTime) }).Contains(sm.Type))
            {
                map.Append(",\"ignore_malformed\":false");
            }
            map.Append("}");
            return map.ToString();
        }

        public static string GetEsMapping(this SimpleMember sm)
        {
            return $@"             ""{sm.Name}"":{GetEsMappingType(sm)}";
        }
        
        public static string GetEsMapping(this ValueObjectMember mb)
        {
            var map = new StringBuilder();
            map.AppendLine($@"    ""{mb.Name}"":{{");

            map.AppendLine(@"        ""type"":  ""object"",");
            map.AppendLine(@"        ""properties"":{");

            var memberMappings = mb.MemberCollection.ToString(",\r\n", m => m.GetEsMapping());
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }
    }
}
