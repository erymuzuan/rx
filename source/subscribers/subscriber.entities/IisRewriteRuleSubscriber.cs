using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class IisRewriteRuleSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_iis_rewrite"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

     
        protected async override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var context = new SphDataContext();
            var query = context.EntityDefinitions.Where(a => a.IsPublished == true);
            var list = await context.GetListAsync(query, a => a.Name);
            var entities = string.Join("|", list.ToArray());

            var path = ConfigurationManager.WebPath + @"\Web.config";
            this.WriteMessage("IIS rewrite into {0}",path);
            var config = XDocument.Load( path);
            var api = config.Descendants("rule").Where(c => c.Attribute("name").Value == "entity.api");
            api.Descendants("match").Single().Attribute("url").Value = string.Format("api/({0})/", entities);

            var search = config.Descendants("rule").Where(c => c.Attribute("name").Value == "entity.search");
            search.Descendants("match").Single().Attribute("url").Value = string.Format("search/({0})/", entities);
            
            config.Save(path);

        }
        
        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
