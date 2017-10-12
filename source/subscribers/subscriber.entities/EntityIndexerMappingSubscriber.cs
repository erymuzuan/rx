using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using static System.IO.File;
using Bespokse.Sph.ElasticsearchRepository;

namespace subscriber.entities
{
    public class EntityIndexerMappingSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_es_mapping_gen";
        public override string[] RoutingKeys => new[] { typeof(EntityDefinition).Name + ".changed.Publish" };
        private MappingBuilder m_builder;
        protected override void OnStart()
        {
            m_builder = new MappingBuilder(m => this.WriteMessage(m), m => this.WriteMessage($"Warning : {m}"),
                m => this.WriteError(m));

            var wc = ConfigurationManager.SphSourceDirectory;
            var type = typeof(EntityDefinition);
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            foreach (var marker in Directory.GetFiles(folder, "*.marker"))
            {
                this.QueueUserWorkItem(MigrateData, Path.GetFileNameWithoutExtension(marker));
                Delete(marker);

            }
            base.OnStart();
        }
        private async void MigrateData(string eid)
        {
            var ed = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{eid}.json"
                .DeserializeFromJsonFile<EntityDefinition>();

            await m_builder.MigrateDataAsync(ed);
        }
        protected override void OnStop()
        {
            base.OnStop();
            m_builder.Dispose();
        }

        protected override async Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var builder = new MappingBuilder(m => this.WriteMessage(m), m => this.WriteWarning(m),
                m=>this.WriteError(m));
            await builder.ReBuildAsync(item);

        }

    }
}
