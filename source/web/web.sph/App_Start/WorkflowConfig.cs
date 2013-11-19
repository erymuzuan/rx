using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.App_Start
{
    public static class WorkflowConfig
    {
        public static void PreStart(HttpServerUtility server)
        {
            var files = Directory.GetFiles(server.MapPath(@"~\bin"), "workflows.*.dll");
            foreach (var s in files)
            {
                var types = Assembly.LoadFrom(s).GetTypes().Where(t => t.BaseType == typeof(Workflow)).ToList();
                types.ForEach(t => XmlSerializerService.RegisterKnownTypes(typeof(Workflow), t));
            }
        }


    }
}