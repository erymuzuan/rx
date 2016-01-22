using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize]
    public class EntityViewController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var view = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            view.Entity = view.EntityDefinitionId;

            if (string.IsNullOrWhiteSpace(view.Id) || view.Id == "0")
                view.Id = $"{view.EntityDefinitionId}-{view.Route.ToIdFormat()}";

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
            var view = context.LoadOneFromSources<EntityView>(x => x.Id == id);
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
            view.Entity = view.EntityDefinitionId;
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
            string type;
            var query = GetElasticsearchQuery(id, out type);
            if (null == query) return Json(new { hits = new { total = 0 } }, JsonRequestBehavior.AllowGet);

            var request = new StringContent(query);
            var url = $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}/{type}/_count";

            using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
            {
                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);

                var responseJson = JObject.Parse(await content.ReadAsStringAsync());
                var count = responseJson.SelectToken("$.count").Value<int>();

                return Content(count.ToString(), "application/json; charset=utf-8");

            }
        }

        private string GetElasticsearchQuery(string id, out string type)
        {
            string path = $"{ConfigurationManager.SphSourceDirectory}\\EntityView\\{id}.json";
            var key = $"count-view-{id}";
            if (!System.IO.File.Exists(path))
            {
                type = null;
                return null;
            }
            var cacheManager = ObjectBuilder.GetObject<ICacheManager>();
            var query = cacheManager.Get<Tuple<string, string>>(key);
            if (null != query)
            {
                type = query.Item1;
                return query.Item2;
            }

            var view = path.DeserializeFromJsonFile<EntityView>();
            type = view.EntityDefinitionId.ToLowerInvariant();

            var json = (@" {
                ""query"": {
                    ""filtered"": {
                        ""filter"":" + Domain.Filter.GenerateElasticSearchFilterDsl(view, view.FilterCollection) + @"
                    }
                }
            }").Replace("config.userName", "\"" + User.Identity.Name + "\"");


            cacheManager.Insert(key, new Tuple<string, string>(type, json), path);

            return json;
        }

        [AllowAnonymous]
        [RxSourceOutputCache(SourceType = typeof(EntityView))]
        public async Task<ActionResult> Dashboard(string id)
        {
            var user = User.Identity.Name;
            var views =
                from f in Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\EntityView\\", "*.json")
                let v = f.DeserializeFromJsonFile<EntityView>()
                where v.IsPublished
                && v.DisplayOnDashboard
                && string.Equals(v.EntityDefinitionId, id, StringComparison.InvariantCultureIgnoreCase)
                select v;

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

            var jsonViews = string.Join(",", list.Select(c => c.ToJsonString(Newtonsoft.Json.Formatting.Indented)));
            return Content($"[{jsonViews}]");
        }
    }
}