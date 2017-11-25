﻿using System;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class MemberExtension
    {
        public static string GetSqlDataType(this Member member)
        {
            switch (member)
            {
                case SimpleMember sm:
                    return GetSqlDataType(sm);
                case ComplexMember cm:
                    return GetSqlDataType(cm);
                case ValueObjectMember vm:
                    return GetSqlDataType(vm);
            }
            return null;
        }

        public static string GetSqlDataType(this ComplexMember member)
        {
            var map = new StringBuilder();
            map.AppendLine($@"    ""{member.Name}"":{{");

            map.AppendLine(@"        ""type"":  ""object"",");
            map.AppendLine(@"        ""properties"":{");

            var memberMappings = member.MemberCollection.ToString(",\r\n", x => x.GetSqlDataType());
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

            var indexed = (sm.IsNotIndexed ? "no" : "analyzed");

            map.Append("{");
            if (sm.Type == typeof(string))
            {
                if (!sm.IsNotIndexed)
                    indexed = (sm.IsAnalyzed ? "analyzed" : "not_analyzed");
            }

            if (sm.Type == typeof(bool))
                indexed = "not_analyzed";

            var boost = sm.Boost;
            if (indexed == "not_analyzed")
                boost = 1;

            var includeAll = (!sm.IsExcludeInAll).ToString().ToLowerInvariant();
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

        public static string GetSqlDataType(this SimpleMember sm)
        {
            return $@"             ""{sm.Name}"":{GetEsMappingType(sm)}";
        }
        
        public static string GetSqlDataType(this ValueObjectMember mb)
        {
            var map = new StringBuilder();
            map.AppendLine($@"    ""{mb.Name}"":{{");

            map.AppendLine(@"        ""type"":  ""object"",");
            map.AppendLine(@"        ""properties"":{");

            var memberMappings = mb.MemberCollection.ToString(",\r\n", m => m.GetSqlDataType());
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }
    }
}
