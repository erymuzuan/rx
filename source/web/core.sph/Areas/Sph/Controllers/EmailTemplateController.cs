using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EmailTemplateController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<EmailTemplate>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.EmailTemplateId });
        }
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
            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = template.EmailTemplateId });

        }
        public async Task<ActionResult> Test()
        {
            var context = new SphDataContext();
            var template = this.GetRequestJson<EmailTemplate>();
            template.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == template.Entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);


            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = template.EmailTemplateId });

        }

        public async Task<ActionResult> Send(string to, string subject, string body)
        {
            var message = new Message
            {
                Subject = subject,
                Body =  body
            };
            var email = ObjectBuilder.GetObject<INotificationService>();
            await email.SendMessageAsync(message,to);
            return Json(true);
        }

        public async Task<ActionResult> Generate(string entity, int id, int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<EmailTemplate>(e => e.EmailTemplateId == templateId);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);

            var buildValidation = await template.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed.Name);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.EntityDefinitionId, ed.Name);
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