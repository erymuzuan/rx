using System;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace domain.test
{
    public static class WebApiControllerHelper
    {
     
        public static HttpContext SetContext(this ApiController controller, string route = "", string queryString = "", string url = "http://localhost:4436/")
        {
            throw new NotImplementedException();
        }

        public static dynamic CreateApiController(this Assembly dll, string controllerTypeName)
        {
            var controllerType = dll.GetType(controllerTypeName);
            if (null == controllerType)
                throw new ArgumentException($"Cannot find the type {controllerTypeName} in {dll}");
            if (!typeof(ApiController).IsAssignableFrom(controllerType))
                throw new InvalidOperationException($"{controllerType} is not assignable from System.Web.Http.ApiController");
            dynamic controller = Activator.CreateInstance(controllerType);
            //MvcControllerHelper.SetContext(controller);
            return controller;
        }

        public static dynamic CreateController(this Assembly dll)
        {
            var types = dll.GetTypes().Where(x => typeof(ApiController).IsAssignableFrom(x));
            var ct = types.FirstOrDefault();
            if (null == ct) return null;
            dynamic controller = Activator.CreateInstance(ct);
            controller.SetContext();

            return controller;
        }
    }
}
