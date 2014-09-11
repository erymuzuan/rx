using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class MessageController : Controller
    {

        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id);
            message.IsRead = true;
            using (var session = context.OpenSession())
            {
                session.Delete(message);
                await session.SubmitChanges("Delete");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content("true");

        }
        public async Task<ActionResult> MarkRead(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id);
            message.IsRead = true;
            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("MarkRead");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content("true");

        }

        public async Task<ActionResult> MarkUnread(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id);
            message.IsRead = false;
            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("MarkUnread");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content("true");

        }

    }
}
