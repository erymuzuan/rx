using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class PartialViewSubscriber : Subscriber<PartialView>
    {
        public override string QueueName => "partial_view_gen";

        public override string[] RoutingKeys => new[] { "PartialView.#.Publish" };

        protected override async Task ProcessMessage(PartialView item, MessageHeaders header)
        {
            await Task.Delay(2000);//temporary workround for source being written
            var html = Path.Combine(ConfigurationManager.WebPath, $"SphApp/views/{item.Route}.html");
            using (var client = new HttpClient())
            {
                var uri = $"{ConfigurationManager.BaseUrl}/Sph/PartialViewRenderer/Html/{item.Route}";
                this.WriteMessage("Rendering {0}", uri);
                var markup = await client.GetStringAsync(uri);
                File.WriteAllText(html, markup.Tidy());
            }
        }

    }
}