using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.entities
{
    public class EntityCodeSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName
        {
            get { return "ed_code_gen"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { typeof(EntityDefinition).Name + ".changed.Publish" }; }
        }

        protected override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var applicationName = ConfigurationManager.ApplicationName;
            var options = new CompilerOptions();
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\web.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll")));


            var result = item.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                this.WriteError(new Exception(result.ToJsonString(Formatting.Indented)));



            return Task.FromResult(0);
        }



    }
}
