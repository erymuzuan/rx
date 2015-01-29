using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize]
    public class EntityViewController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var view = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();

            if (string.IsNullOrWhiteSpace(view.Id) || view.Id == "0")
                view.Id = view.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = view.Id, message = "Your view has been successfully saved" });
        }

        [HttpDelete]
        public async Task<ActionResult> Index(string id)
        {
            var context = new SphDataContext();
            var view = await context.LoadOneAsync<EntityView>(x => x.Id == id);
            if (null == view) return new HttpNotFoundResult("Cannot find EntityView with id " + id);



            using (var session = context.OpenSession())
            {
                session.Delete(view);
                await session.SubmitChanges("Delete");
            }
            return Json(new { success = true, status = "OK", id = view.Id });
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
            return Json(new { success = true, status = "OK", message = "Your view has been successfully depublished", id = ed.Id });


        }

        public async Task<ActionResult> Publish()
        {
            var view = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == view.EntityDefinitionId);

            var buildValidation = await view.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            view.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", id = view.Id, message = "Your view has been successfully published" });
        }

        [NoCache]
        public async Task<ActionResult> Count(string id)
        {
            var context = new SphDataContext();
            var view = await context.LoadOneAsync<EntityView>(e => e.Id == id);
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == view.EntityDefinitionId);
            var type = ed.Name.ToLowerInvariant();

            var json = (@" {
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

        [NoCache]
        public async Task<ActionResult> Dashboard(string id)
        {
            var user = User.Identity.Name;
            var context = new SphDataContext();
            var entity = await context.LoadOneAsync<EntityDefinition>(e => e.Name == id);
            var query = context.EntityViews.Where(v => v.EntityDefinitionId == entity.Id
                && v.IsPublished == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            var views = new ObjectCollection<EntityView>();
            views.AddRange(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                views.AddRange(lo.ItemCollection);
            }

            var list = new ObjectCollection<EntityView>();
            foreach (var v in views)
            {
                if (!v.Performer.Validate()) continue;
                if (v.RouteParameterCollection.Any()) continue;

                if (v.Performer.IsPublic)
                {
                    list.Add(v);
                    continue;
                }
                var users = await v.Performer.GetUsersAsync(v);
                if (users.Contains(user))
                    list.Add(v);
            }

            return Content("[" +

               string.Join(",", list.Select(c => c.ToJsonString(Newtonsoft.Json.Formatting.Indented)))
                + "]");
        }
    }
}