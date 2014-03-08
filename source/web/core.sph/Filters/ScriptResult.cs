using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Web.Areas.App.Controllers;

namespace Bespoke.Sph.Web.Filters
{
    public class ScriptResult : ViewResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (String.IsNullOrEmpty(ViewName))
            {
                ViewName = context.RouteData.GetRequiredString("action");
            }

            ViewEngineResult result = null;

            if (View == null)
            {
                result = FindView(context);
                View = result.View;
            }

            TextWriter writer = context.HttpContext.Response.Output;

            var viewContext = new ViewContext(context, View, ViewData, TempData, writer);
            View.Render(viewContext, writer);

            if (result != null)
            {
                result.ViewEngine.ReleaseView(context, View);
            }
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = BaseAppController.APPLICATION_JAVASCRIPT;

        }
    }
}