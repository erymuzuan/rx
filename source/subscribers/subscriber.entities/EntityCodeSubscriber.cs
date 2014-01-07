using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

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
            get { return new[] { typeof(EntityDefinition).Name + ".#.Save" }; }
        }

        protected  override Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {  var applicationName = ConfigurationManager.ApplicationName;

            var code = new StringBuilder("");
            code.AppendLine("using System;");
            code.AppendLinf("namespace Bespoke.{0}.Domain", applicationName);
            code.AppendLine("{");
            code.AppendLinf("   public class {0} : Entity", item.Name);
            code.AppendLine("   {");



            code.AppendLine("   }");
            code.AppendLine("}");
            
            return Task.FromResult(0);
        }



    }
}
