using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public async Task<ActionResult> ComplaintFormHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

            return View(template);
        }    
        
      

         public ActionResult TemplateComplaintHtml()
        {
            return RedirectToAction("Complaint", "Template");
     
        }

         public async Task<ActionResult> ComplaintDetailHtml(int templateId)
         {
             var context = new SphDataContext();
             var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

             return View(template);
         }

         public ActionResult ComplaintListHtml()
         {
             return View();
         }

         public async Task<ActionResult> ComplaintAssignHtml(int templateId)
         {
             var context = new SphDataContext();
             var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

             return View(template);
         }

         public ActionResult ComplaintListJs()
         {
             return View();
         }  
        
        public async Task<ActionResult> ComplaintCloseHtml(int templateId)
        {
            var context = new SphDataContext();
            var template = await context.LoadOneAsync<ComplaintTemplate>(t => t.ComplaintTemplateId == templateId);

            return View(template);
        }
    }
}
