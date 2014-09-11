using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

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


        protected override void OnStart()
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = typeof(EntityDefinition);
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var marker in Directory.GetFiles(folder, "*.marker"))
            {
                this.QueueUserWorkItem(MigrateData, Path.GetFileNameWithoutExtension(marker));
                File.Delete(marker);

            }
            base.OnStart();
        }

        private async void MigrateData(string name)
        {
            await MigrateDataAsycn(name);

        }
        private async Task MigrateDataAsycn(string name)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(name);
            this.WriteMessage("Starting data migration for " + name);
            var connectionString = ConfigurationManager.ConnectionStrings["sph"].ConnectionString;
            var applicationName = ConfigurationManager.ApplicationName;

            var taskBuckets = new List<Task>();

            using (var conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();//migrate
                var readSql = string.Format("SELECT [{0}Id],[Json] FROM [{1}].[{0}]", name, applicationName);
                this.WriteMessage(readSql);


                using (var cmd = new SqlCommand(readSql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var json = reader.GetString(1);
                            this.WriteMessage("Migrating {0} : {1}", name, id);
                            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                            dynamic ent = JsonConvert.DeserializeObject(json, setting);
                            ent.SetId(id);

                            var task = IndexItemToElasticSearchAsync(ent);
                            taskBuckets.Add(task);
                            if (taskBuckets.Count > 10)
                            {
                                await Task.WhenAll(taskBuckets);
                                taskBuckets.Clear();
                            }
                        }

                    }
                }
            }
            if (taskBuckets.Any())
                await Task.WhenAll(taskBuckets);


            Console.ResetColor();
        }

        private async Task IndexItemToElasticSearchAsync(Entity item)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var id = item.Id;
            if (item.GetType().Namespace == typeof(Entity).Namespace) return;// just custom entity


            var url = string.Format("{0}/{1}/{2}",
                ConfigurationManager.ApplicationName.ToLowerInvariant(),
                item.GetType().Name.ToLowerInvariant(),
                id);

            using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
            {
                var response = await client.PostAsync(url, content);
                if (null != response)
                {
                    Console.Write(".");
                }
            }
            Console.ResetColor();


        }
        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            // compare
            var map = this.GetMapping(item);
            if (this.Compare(item, map)) return;

            this.WriteMessage("There are differences from the existing ElasticSearch mapping");
            var result = await PutMappingAsync(item);
            if (result)
            {
                this.SaveMap(item, map);
                this.SaveMigrationMarker(item);
            }

        }

        public async Task<bool> PutMappingAsync(EntityDefinition item)
        {
            var url = string.Format("{0}/_mapping/{1}", ConfigurationManager.ApplicationName.ToLowerInvariant(), item.Name.ToLowerInvariant());

            var map = this.GetMapping(item);
            var content = new StringContent(map);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PutAsync(url, content);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Console.Write(".");
                    // creates the index for the 1st time
                    await client.PutAsync(ConfigurationManager.ApplicationName.ToLowerInvariant(), new StringContent(""));
                    return await this.PutMappingAsync(item);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var rc = response.Content as StreamContent;
                    var text = string.Empty;
                    if (null != rc)
                        text = await rc.ReadAsStringAsync();

                    this.WriteError(new Exception(" Error creating Elastic search map for " + item.Name + "/r/n" + text));
                }

                return response.StatusCode == HttpStatusCode.OK;
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

        private void SaveMigrationMarker(EntityDefinition item)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);

            var marker = Path.Combine(folder, item.Name + ".marker");
            File.WriteAllText(marker, DateTime.Now.ToString(CultureInfo.InvariantCulture));


        }
    }
}
