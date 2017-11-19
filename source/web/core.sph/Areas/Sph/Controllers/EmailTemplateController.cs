using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("email-template")]
    public class EmailTemplateController : Controller
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var template = this.GetRequestJson<EmailTemplate>();
            if (template.IsNewItem)
                template.Id = template.Name.ToIdFormat();
            
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = template.Id });
        }

        [HttpPost]
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var template = this.GetRequestJson<EmailTemplate>();
            template.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == template.Entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(template);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your template has been successfully published", id = template.Id });

        }

        [HttpPost]
        [Route("test")]
        public async Task<ActionResult> Test()
        {
            var context = new SphDataContext();
            var template = this.GetRequestJson<EmailTemplate>();
            template.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == template.Entity);

           

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);


            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = template.Id });

        }

        [HttpPost]
        [Route("send")]
        public async Task<ActionResult> Send(string to, string subject, string body)
        {
            var message = new Message
            {
                Subject = subject,
                Body =  body,
                Id = Strings.GenerateId()
            };
            var email = ObjectBuilder.GetObject<INotificationService>();
            await email.SendMessageAsync(message,to);
            return Json(true);
        }

        [HttpGet]
        [Route("generate/{entity}/{id}/{templateId}")]
        public async Task<ActionResult> Generate(string entity, string id, string templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<EmailTemplate>(e => e.Id == templateId);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);


            var resolved = ObjectBuilder.GetObject<ICustomEntityDependenciesResolver>()
                .ResolveRepository(ed);
            var item = await resolved.Implementation.LoadOneAsync(id);

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var subject = await razor.GenerateAsync(template.SubjectTemplate, item);
            var body = await razor.GenerateAsync(template.BodyTemplate, item);

            return Json(new { success = true, status = "OK", message = "Your template has been successfully generated", subject, body }, JsonRequestBehavior.AllowGet);

        }
    }
}