using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static string GetElasticsearchMapping(this EntityDefinition ed)
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLine($"    \"{ed.Name.ToLowerInvariant()}\":{{");
            map.AppendLine("        \"properties\":{");
            // add entity default properties
            map.AppendLine(@"            ""CreatedBy"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""ChangedBy"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""WebId"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""CreatedDate"": {""type"": ""date""},");
            map.AppendLine(@"            ""ChangedDate"": {""type"": ""date""},");

            var memberMappings = ed.MemberCollection.ToString(",\r\n", d => d.GetEsMapping());
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }
    }
}