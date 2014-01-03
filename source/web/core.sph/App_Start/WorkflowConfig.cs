using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Optimization;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.App_Start;

namespace Bespoke.Sph.Web.App_Start
{
    public static class WorkflowConfig
    {
        public static void Register(HttpServerUtility server)
        {
            var files = Directory.GetFiles(server.MapPath(@".\bin\"), "workflows.*.dll");
            foreach (var s in files)
            {
                var types = Assembly.LoadFrom(s).GetTypes().Where(t => t.BaseType == typeof(Workflow)).ToList();
                types.ForEach(t => XmlSerializerService.RegisterKnownTypes(typeof(Workflow), t));

            }

        }
    }
}