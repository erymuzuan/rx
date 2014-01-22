using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class EntityIndexerMappingSubscriber : Subscriber<EntityDefinition>
    {
        
        public override string QueueName
        {
            get { return "ed_es_mapping_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        public string GetMapping(EntityDefinition item)
        {
            var map = new StringBuilder();
            map.AppendLine("{");
            map.AppendLinf("    \"{0}\":{{", item.Name.ToLowerInvariant());
            map.AppendLine("        \"properties\":{");
            // add entity default properties
            map.AppendLine("            \"CreatedBy\": {\"type\": \"string\", \"index\":\"no\"},");
            map.AppendLine("            \"ChangedBy\": {\"type\": \"string\", \"index\":\"no\"},");
            map.AppendLine("            \"CreatedDate\": {\"type\": \"date\"},");
            map.AppendLine("            \"ChangedDate\": {\"type\": \"date\"},");

            var memberMappings = string.Join(",", item.MemberCollection.Select(d => d.GetMemberMappings()).SelectMany(m => m));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            map.AppendLine("}");
            return map.ToString();
        }

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var applicationName = ConfigurationManager.ApplicationName;

            var indexUrl = ConfigurationManager.ElasticSearchHost + "/" + applicationName.ToLowerInvariant();
            var url = indexUrl + string.Format("/{0}/_mapping", item.Name.ToLowerInvariant());

            var map = this.GetMapping(item);
            var content = new StringContent(map);
            var client = new HttpClient();
            var response = await client.PutAsync(url, content);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // creates the index for the 1st time
                await client.PutAsync(indexUrl, new StringContent(""));
                await this.ProcessMessage(item, header);
            }

        }



    }
}
