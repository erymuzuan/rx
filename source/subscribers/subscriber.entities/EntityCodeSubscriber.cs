using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Member> GetFiltarableMembers(string parent, IList<Member> members)
        {
            var filterables = members.Where(m => m.IsFilterable)
                .Where(m => m.TypeName != "System.Object, mscorlib")
                .Where(m => m.TypeName != "System.ArrayList, mscorlib")
                .ToList();
            var list = members.Where(m => m.TypeName == "System.Object, mscorlib")
                .Select(m => this.GetFiltarableMembers(parent + m.Name + ".", m.MemberCollection))
                .SelectMany(m => m)
                .ToList();
            filterables.AddRange(list);

            filterables.Where(m => string.IsNullOrWhiteSpace(m.FullName))
                .ToList().ForEach(m => m.FullName = parent + m.Name);

            return filterables;

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
