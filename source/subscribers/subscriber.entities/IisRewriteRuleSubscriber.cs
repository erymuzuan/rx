using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class IisRewriteRuleSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_iis_rewrite";

        public override string[] RoutingKeys => new[] { typeof(EntityDefinition).Name + ".changed.Publish" };


        protected  override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>(x => x.IsPublished).Select( a => a.Name);
            var entities = string.Join("|", list.ToArray());

            var path = ConfigurationManager.WebPath + @"\Web.config";
            this.WriteMessage("IIS rewrite into {0}", path);
            var config = XDocument.Load(path);
            var api = config.Descendants("rule").Where(c => c.Attribute("name").Value == "entity.api").ToArray();
            var apiUrl = api.Descendants("match").Single().Attribute("url").Value;
            if (apiUrl != $"api/({entities})/")
            {
                api.Descendants("match").Single().Attribute("url").Value = $"api/({entities})/";
                config.Save(path);
            }

            var search = config.Descendants("rule").Where(c => c.Attribute("name").Value == "entity.search").ToArray();

            var searchUrl = search.Descendants("match").Single().Attribute("url").Value;
            if (searchUrl != $"search/({entities})/")
            {
                search.Descendants("match").Single().Attribute("url").Value = $"search/({entities})/";
                config.Save(path);
            }

            return Task.FromResult(0);

        }

        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
