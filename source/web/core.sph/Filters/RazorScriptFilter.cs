using System.Web.Mvc;
using Bespoke.Sph.Web.Areas.App.Controllers;

namespace Bespoke.Sph.Web.Filters
{
    public class RazorScriptFilter : ActionFilterAttribute
    {
        
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext.Result is ViewResult)
            {
                filterContext.RequestContext.HttpContext.Response.Filter = new RazorScriptStream(
                    filterContext.RequestContext.HttpContext.Response.Filter);
                filterContext.RequestContext.HttpContext.Response.ContentType = BaseAppController.APPLICATION_JAVASCRIPT;
            }
            base.OnResultExecuted(filterContext);
        }
    }
}