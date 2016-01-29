using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace domain.test
{
    public static class MvcControllerHelper
    {
        public static void SetContext(this Controller controller)
        {
            var request = new HttpRequest("", "http://example.com/", "");
            var response = new HttpResponse(TextWriter.Null);
            var httpContext = new HttpContextWrapper(new HttpContext(request, response));
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
        }

        public static dynamic CreateController(this Assembly dll, string controllerTypeName)
        {
            var controllerType = dll.GetType(controllerTypeName);
            if(null == controllerType)
                throw new ArgumentException("Cannot find the type " + controllerTypeName + " in " + dll);
            if(!typeof(Controller).IsAssignableFrom(controllerType))
                throw new InvalidOperationException($"{0} is not assignable from System.Web.Mvc.Controller");
            dynamic controller = Activator.CreateInstance(controllerType);
            MvcControllerHelper.SetContext(controller);
            return controller;
        }

        public static dynamic CreateController(this Assembly dll)
        {
            var types = dll.GetTypes().Where(x => typeof(Controller).IsAssignableFrom(x));
            var ct = types.FirstOrDefault();
            if (null == ct) return null;
            dynamic controller = Activator.CreateInstance(ct);
            controller.SetContext();

            return controller;
        }
    }
}
