using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.diagnostics;
using Newtonsoft.Json.Linq;

namespace es.diagnostics
{
    [Export(typeof(IBuildDiagnostics))]
    public sealed class ElasticsearchMappingDiagnostics : BuilDiagnostic
    {
        public string GetMapping(EntityDefinition item)
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLinf("    \"{0}\":{{", item.Name.ToLowerInvariant());
            map.AppendLine("        \"properties\":{");
            // add entity default properties
            map.AppendLine("            \"CreatedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"ChangedBy\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"WebId\": {\"type\": \"string\", \"index\":\"not_analyzed\"},");
            map.AppendLine("            \"CreatedDate\": {\"type\": \"date\"},");
            map.AppendLine("            \"ChangedDate\": {\"type\": \"date\"},");

            var memberMappings = string.Join(",\r\n", item.MemberCollection.Select(d => d.GetMemberMappings()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }


        public override async Task<BuildError[]> ValidateWarningsAsync(EntityDefinition ed)
        {
            var warnings = new List<BuildError>();
            string entity = ed.Name.ToLower();
            var mapCurrent = GetMapping(ed);
            var currentMap = JObject.Parse(mapCurrent);

            JObject esMap;

            using (var client = new HttpClient())
            {
                var text2 = await client.GetStringAsync($"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/_mapping/" + entity);
                esMap = JObject.Parse(text2);
            }

            JToken fields = currentMap[entity]["properties"];
            foreach (var field in fields)
            {
                var element = field.First;
                var member = field.Path.Replace(entity + ".properties.", "");
                var es = esMap.SelectToken($"{ConfigurationManager.ElasticSearchIndex}.mappings.{field.Path}");


                if (element["type"].Value<string>() != "boolean")
                {

                    var includeInAll = element["include_in_all"].MapEquals<bool>(es["include_in_all"]);
                    if (!includeInAll)
                        warnings.Add(new BuildError(ed.WebId, $"{member} has different include_in_all from mapping and Elasticsearch"));

                    var boost = element["boost"].MapEquals<int>(es["boost"]);
                    if (!boost)
                        warnings.Add(new BuildError(ed.WebId, $"'{member}' has different boost from mapping and Elasticsearch"));
                }

            }
            return warnings.ToArray();
        }

        public override async Task<BuildError[]> ValidateErrorsAsync(EntityDefinition ed)
        {
            var errors = new List<BuildError>();
            string entity = ed.Name.ToLower();
            var text1 = GetMapping(ed);
            var currentMap = JObject.Parse(text1);

            JObject esMap;

            using (var client = new HttpClient())
            {
                var text2 = await client.GetStringAsync($"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/_mapping/" + entity);
                esMap = JObject.Parse(text2);
            }

            JToken fields = currentMap[entity]["properties"];
            foreach (var field in fields)
            {
                var map = field.First;


                var member = field.Path.Replace(entity + ".properties.", "");
                var es = esMap.SelectToken($"{ConfigurationManager.ElasticSearchIndex}.mappings.{field.Path}");

                var type = map["type"].MapEquals<string>(es["type"]);
                if (map["type"].Value<string>() == "object")
                {
                    // TODO : recursely checking the inner object - complex/collection
                    type = true;
                }
                if (!type) errors.Add(new BuildError(ed.WebId, $"{member} have type different from mapping and Elasticsearch"));

                if (map["type"].Value<string>() != "boolean")
                {
                    var index = map["index"].MapEquals<string>(es["index"]);
                    if (!index)
                        errors.Add(new BuildError(ed.WebId, $"{member} has different index from mapping and Elasticsearch"));

                }

            }
            return errors.ToArray();

        }


    }
}
