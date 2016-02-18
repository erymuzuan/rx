using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class WorkflowController : Controller
    {
        public async Task<ActionResult> StartWorkflow()
        {
            var json = this.GetRequestJson<string>();
            var context = new SphDataContext();
            dynamic obj = JsonConvert.DeserializeObject(json);
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == "1");
            var wf = new Workflow();
            foreach (var w in wd.VariableDefinitionCollection)
            {
                var name = w.Name;
                var value = obj[w.Name];
                var v = new VariableValue { Name = name, Value = value };
                wf.VariableValueCollection.Add(v);
            }

            return Content("");
        }


        public ActionResult InvalidState()
        {
            return View();
        }

        public async Task<ActionResult> GetPendingTasks(string id)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(w => w.Id == id);
            var tracker = await wf.GetTrackerAsync();
            var pendingTasks = await tracker.GetPendingTasksAsync();


            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(pendingTasks));

        }

        public async Task<ActionResult> GetPendingTasksByUser(string id)
        {
            var userName = id;
            var query = new
            {
                query = new
                {
                    term = new
                    {
                        Performers = new
                        {
                            value = userName
                        }
                    }
                }
            };
            var json = JsonConvert.SerializeObject(query);
            var request = new StringContent(json);
            var url =  $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/pendingtask/_search";

            var client = new HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());

        }


    }
}