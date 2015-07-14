using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [RoutePrefix("sph-message")]
    [Authorize]
    public class MessageController : Controller
    {
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id && m.UserName == User.Identity.Name);
            if (null == message)
                return HttpNotFound($"Cannot find message {id} for {User.Identity.Name}");

            message.IsRead = true;
            using (var session = context.OpenSession())
            {
                session.Delete(message);
                await session.SubmitChanges("Delete");
            }
            return Json(true);

        }

        [HttpPost]
        [Route("mark-read")]
        public async Task<ActionResult> MarkRead(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id && m.UserName == User.Identity.Name);
            if (null == message)
                return HttpNotFound($"Cannot find message {id} for {User.Identity.Name}");
            message.IsRead = true;
            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("MarkRead");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content("true");

        }

        [HttpPost]
        [Route("mark-unread")]
        public async Task<ActionResult> MarkUnread(string id)
        {
            var context = new SphDataContext();
            var message = await context.LoadOneAsync<Message>(m => m.Id == id && m.UserName == User.Identity.Name);
            if (null == message)
                return HttpNotFound($"Cannot find message {id} for {User.Identity.Name}");

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
