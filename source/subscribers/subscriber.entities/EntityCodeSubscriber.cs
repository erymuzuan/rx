using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
namespace subscriber.entities
{
    public class EntityCodeSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_code_gen";

        public override string[] RoutingKeys => new[] { typeof(EntityDefinition).Name + ".changed.Publish" };

        protected override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var options = new CompilerOptions();
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(ConfigurationManager.WebPath + @"\bin\Newtonsoft.Json.dll"));

            var codes = item.GenerateCode();
            var sources = item.SaveSources(codes);
            var result = item.Compile(options, sources);

            result.Errors.ForEach(Console.WriteLine);
            if (!result.Result)
                this.WriteError(new Exception(result.ToJsonString(Formatting.Indented)));
            

            return Task.FromResult(0);
        }






    }
}
