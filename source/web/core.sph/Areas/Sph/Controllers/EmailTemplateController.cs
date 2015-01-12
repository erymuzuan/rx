using System;
using System.Reflection;
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
            var et = this.GetRequestJson<EmailTemplate>();
            if (et.IsNewItem && !string.IsNullOrWhiteSpace(et.WebId))
                et.Id = et.WebId;
            if (et.IsNewItem && string.IsNullOrWhiteSpace(et.WebId))
            {
                et.Id = Guid.NewGuid().ToString();
                et.WebId = et.Id;
            }


            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(et);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = et.Id });
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

        [HttpPost]
        [Route("generate")]
        public async Task<ActionResult> Generate(string entity, int id, string templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<EmailTemplate>(e => e.Id == templateId);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed.Name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, ed.Name);
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine("Cannot create type " + edTypeName);


            var reposType = sqlRepositoryType.MakeGenericType(edType);
            dynamic repository = Activator.CreateInstance(reposType);
            var item = await repository.LoadOneAsync(id);

            var razor = ObjectBuilder.GetObject<ITemplateEngine>();
            var subject = await razor.GenerateAsync(template.SubjectTemplate, item);
            var body = await razor.GenerateAsync(template.BodyTemplate, item);

            return Json(new { success = true, status = "OK", message = "Your template has been successfully generated", subject, body });

        }
    }
}