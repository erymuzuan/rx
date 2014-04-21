using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityViewController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.EntityViewId });
        }
        
        public async Task<ActionResult> Depublish()
        {
            var context = new SphDataContext();
            var ed = this.GetRequestJson<EntityView>();

            ed.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your view has been successfully depublished", id = ed.EntityViewId });


        }
        public async Task<ActionResult> Publish()
        {
            var view = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.EntityDefinitionId == view.EntityDefinitionId);

            var buildValidation = await view.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            view.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", id = view.EntityViewId });
        }

        public async Task<ActionResult> Count(int id)
        {
            var context = new SphDataContext();
            var view = await context.LoadOneAsync<EntityView>(e => e.EntityViewId == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.EntityDefinitionId == view.EntityDefinitionId);
            var type = ed.Name.ToLowerInvariant();

            var json =( @" {
                ""query"": {
                    ""filtered"": {
                        ""filter"":" + view.GenerateElasticSearchFilterDsl() + @"
                    }
                }
            }").Replace("config.userName", "\"" + User.Identity.Name + "\"");
            Console.WriteLine(json);
            var request = new StringContent(json);
            var url = string.Format("{0}/{1}/_search", ConfigurationManager.ApplicationName.ToLowerInvariant(), type);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(await content.ReadAsStringAsync());

            }
        }
    }
}