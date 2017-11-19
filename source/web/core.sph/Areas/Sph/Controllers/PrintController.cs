using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [RoutePrefix("print-out")]
    public class PrintController : Controller
    {
        [Route("{entity}/{id}")]
        public async Task<ActionResult> Index(string entity, string id)
        {
            var vm = new PrintViewModel();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);
            var form = await context.LoadOneAsync<EntityForm>(f => f.EntityDefinitionId == ed.Id && f.IsDefault == true);
            vm.FormDesign = form.FormDesign;
            

            var resolved = ObjectBuilder.GetObject<ICustomEntityDependenciesResolver>()
                .ResolveRepository(ed);
            var item = await resolved.Implementation.LoadOneAsync(id);


            vm.Item = item;
            vm.Name = item.ToString();

            return View(vm);
        }
    }
}