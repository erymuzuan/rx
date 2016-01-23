using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Api
{
    [RoutePrefix("api/docs")]
    public class ApiDocumentationController : Controller
    {
        [Route("")]
        public ActionResult Index(string entity, string operation)
        {
            var context = new SphDataContext();
            var list = context.LoadFromSources<EntityDefinition>(x => x.IsPublished);
            return View(list);
        }

        [Route("{entity}")]
        public ActionResult Entity(string entity)
        {
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Id == entity);
            if(null == ed)return HttpNotFound("Cannot find EntityDefinition: " + entity);

            var vm = new OperationDocumentViewModel(ed);
            return View(vm);
        }
    }
}