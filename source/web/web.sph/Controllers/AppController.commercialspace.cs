using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Web.ViewModels;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public async Task<ActionResult> SpaceDetailPublicHtml(int templateId)
        {
            var context = new SphDataContext();
            var template  = await  context.LoadOneAsync<SpaceTemplate>(t => t.SpaceTemplateId == templateId);
            Console.WriteLine("TemplateId {0}", templateId);
            var vm = new SpaceDetailViewModel
            {
                Template = template,
                ApplicationTemplates = new ApplicationTemplate[]{}
            };

            return View(vm);
        }

        public async Task<ActionResult> ApplicationDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var templateTask =  context.LoadOneAsync<ApplicationTemplate>(t => t.ApplicationTemplateId == templateId);

            await Task.WhenAll(templateTask);
         
            return View(await templateTask);
        }


        public async Task<ActionResult> SpaceDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var templateTask =  context.LoadOneAsync<SpaceTemplate>(t => t.SpaceTemplateId == templateId);
            var applicationTemplatesTask =  context.LoadAsync(context.ApplicationTemplates);

            await Task.WhenAll(templateTask, applicationTemplatesTask);
            var vm = new SpaceDetailViewModel
                {
                    Template = await templateTask,
                    ApplicationTemplates = (await applicationTemplatesTask).ItemCollection.ToArray()
                };


            return View(vm);
        }

    }
}
