using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class PrintController : Controller
    {
        public async Task<ActionResult> Index(int id, string entity)
        {
            var vm = new PrintViewModel();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Name == entity);
            var form =
                await
                    context.LoadOneAsync<EntityForm>(
                        f => f.EntityDefinitionId == ed.Id && f.IsDefault == true);
            vm.FormDesign = form.FormDesign;



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

            vm.Item = item;
            vm.Name = item.ToString();

            return View(vm);
        }
    }
}