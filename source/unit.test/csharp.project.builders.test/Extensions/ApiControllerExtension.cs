using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Bespoke.Sph.Tests.Extensions
{


    public static class ApiControllerExtension
    {
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
