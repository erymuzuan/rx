using System;
using System.IO;
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

        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var url = string.Format("{0}/_mapping/{1}", ConfigurationManager.ApplicationName.ToLowerInvariant(), item.Name.ToLowerInvariant());

            var map = this.GetMapping(item);
            var content = new StringContent(map);
            // compare
            if (this.Compare(item, map)) return;

            this.WriteMessage("There are differences from the existing ElasticSearch mapping");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PutAsync(url, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.Write(".");
                    // creates the index for the 1st time
                    await client.PutAsync(ConfigurationManager.ApplicationName.ToLowerInvariant(), new StringContent(""));
                    await this.ProcessMessage(item, header);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var rc = response.Content as StreamContent;
                    var text = string.Empty;
                    if (null != rc)
                        text = await rc.ReadAsStringAsync();

                    this.WriteError(new Exception(" Error creating Elastic search map for " + item.Name + "/r/n" + text));
                }
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this.SaveMap(item, map);
                }
            }

        }

        private bool Compare(EntityDefinition item, string map)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = typeof(EntityDefinition);
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Name + ".mapping");
            if (!File.Exists(file)) return false;
            return File.ReadAllText(file) == map;

        }

        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }


        private void SaveMap(EntityDefinition item, string map)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Name + ".mapping");
            File.WriteAllText(file, map);


        }



    }
}
