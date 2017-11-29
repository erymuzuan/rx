using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class EntityDefinitionExtension
    {
        public static string GetSqlTableSchema(this EntityDefinition ed)
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLine($"    \"{ed.Name.ToLowerInvariant()}\":{{");
            map.AppendLine("        \"properties\":{");
            // add entity default properties
#if ES5
            map.AppendLine(@"            ""CreatedBy"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""ChangedBy"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""WebId"": {""type"": ""keyword"", ""index"":""not_analyzed""},");
           
#elif ES1_7
            map.AppendLine(@"            ""CreatedBy"": {""type"": ""string"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""ChangedBy"": {""type"": ""string"", ""index"":""not_analyzed""},");
            map.AppendLine(@"            ""WebId"": {""type"": ""string"", ""index"":""not_analyzed""},");
#endif
            map.AppendLine(@"            ""CreatedDate"": {""type"": ""date""},");
            map.AppendLine(@"            ""ChangedDate"": {""type"": ""date""},");

            var memberMappings = ed.MemberCollection.ToString(",\r\n", d => d.GetSqlDataType());
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }
    }
}