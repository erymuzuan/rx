using System;
using System.Web.Mvc;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public class MobileCapableWebFormViewEngine : RazorViewEngine
    {
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            ViewEngineResult result = null;
            var request = controllerContext.HttpContext.Request;

            var android = UserAgentIs(controllerContext, "Android");
            if (request.Browser.IsMobileDevice || android)
            {
                result = base.FindView(controllerContext, "Mobile/" + viewName, masterName, useCache);
            }

            //Fall back to desktop view if no other view has been selected
            if (result == null || result.View == null)
            {
                result = base.FindView(controllerContext, viewName, masterName, useCache);
            }

            return result;
        }

        public bool UserAgentIs(ControllerContext controllerContext, string userAgentToTest)
        {
            if (null == controllerContext) return false;
            if (null == controllerContext.HttpContext) return false;
            if (null == controllerContext.HttpContext.Request) return false;
            if (null == controllerContext.HttpContext.Request.UserAgent) return false;
            return (controllerContext.HttpContext.Request.UserAgent.IndexOf(userAgentToTest,
                            StringComparison.OrdinalIgnoreCase) > 0);
        }
    }
}