using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("wf-designer")]
    public class WorkflowDesignerController : Controller
    {
        [ImportMany("ActivityDesigner", typeof(Activity), AllowRecomposition = true)]
        public Lazy<Activity, IDesignerMetadata>[] ToolboxItems { get; set; }

        [Route("toolbox-items")]
        public ActionResult GetToolboxItems()
        {
            if (null == this.ToolboxItems)
                ObjectBuilder.ComposeMefCatalog(this);
            var actions = from a in this.ToolboxItems
                          select string.Format(@"
{{
    ""designer"" : {0},
    ""activity"" : {1}
}}", JsonConvert.SerializeObject(a.Metadata), a.Value.ToJsonString());


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);
        }
    }
}