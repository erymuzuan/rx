using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Jsbeautifier;

namespace subscriber.entities
{
    public class FormDialogSubscriber : Subscriber<FormDialog>
    {
        public override string QueueName => "dialog_gen";

        public override string[] RoutingKeys => new[] { $"{nameof(FormDialog)}.changed.Publish" };

        protected override async Task ProcessMessage(FormDialog item, MessageHeaders header)
        {
            await Task.Delay(2000);//temporay workround for source being written
            var html = Path.Combine(ConfigurationManager.WebPath, "SphApp/views/" + item.Route.ToLower() + ".html");
            using (var client = new HttpClient())
            {
                var uri = ConfigurationManager.BaseUrl + "/Sph/FormDialogRenderer/Html/" + item.Route;
                this.WriteMessage("Rendering {0}", uri);
                var markup = await client.GetStringAsync(uri);
                File.WriteAllText(html, markup.Tidy());
            }


            var js = Path.Combine(ConfigurationManager.WebPath, "SphApp/viewmodels/" + item.Route.ToLower() + ".js");
            using (var client = new HttpClient())
            {
                var script = await client.GetStringAsync(ConfigurationManager.BaseUrl + "/Sph/FormDialogRenderer/Js/" + item.Route);
                var b = new Beautifier();
                script = b.Beautify(script);
                File.WriteAllText(js, script);
            }

        }



    }
}