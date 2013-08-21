using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public async Task<ActionResult> ApplicationDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var templateTask =  context.LoadOneAsync<ApplicationTemplate>(t => t.ApplicationTemplateId == templateId);

            await Task.WhenAll(templateTask);
         
            return View(await templateTask);
        }
        public async Task<ActionResult> CommercialSpaceDetailHtml(int templateId)
        {
            var context = new SphDataContext();
            var templateTask =  context.LoadOneAsync<CommercialSpaceTemplate>(t => t.CommercialSpaceTemplateId == templateId);
            var applicationTemplatesTask =  context.LoadAsync(context.ApplicationTemplates);

            await Task.WhenAll(templateTask, applicationTemplatesTask);
            var vm = new CommercialSpaceDetailViewModel
                {
                    Template = await templateTask,
                    ApplicationTemplates = (await applicationTemplatesTask).ItemCollection.ToArray()
                };


            return View(vm);
        }

    }

    public class CommercialSpaceDetailViewModel
    {
        public CommercialSpaceTemplate Template { get; set; }

        public ApplicationTemplate[] ApplicationTemplates { get; set; }
    }
}
