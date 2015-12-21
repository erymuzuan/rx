using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectMember : Member
    {
        public override string GetDefaultValueCode(int count)
        {
            return this.AllowMultiple ? null : $"this.{Name} = new {ValueObjectName}();";
        }


        public override string GeneratedCode(string padding = "      ")
        {
            if (this.AllowMultiple)
                return padding +
                       $"public ObjectCollection<{ValueObjectName}> {Name} {{get;}} = new ObjectCollection<{ValueObjectName}>();";
            return padding + $"public {ValueObjectName} {Name} {{ get; set;}}";
        }


        public override string GeneratedCustomClass()
        {
            var code = new StringBuilder();
            code.AppendLine($"   public class {ValueObjectName}: DomainObject");

            code.AppendLine("   {");
            // ctor
            code.AppendLine($"       public {ValueObjectName}()");
            code.AppendLine("       {");
            code.AppendLinf("           var rc = new RuleContext(this);");


            var context = new SphDataContext();
            var vod = context.LoadOne<ValueObjectDefinition>(d => d.Name == this.ValueObjectName);
            this.MemberCollection.ClearAndAddRange(vod.MemberCollection);

            var count = 0;
            foreach (var member in this.MemberCollection)
            {
                count++;
                var defaultValueCode = member.GetDefaultValueCode(count);
                code.AppendLine(defaultValueCode);
            }
            code.AppendLine("       }");
            foreach (var member in this.MemberCollection)
            {
                code.AppendLine(member.GeneratedCode());

            }
            code.AppendLine("   }");
            foreach (var member in this.MemberCollection.Where(x =>x.GetType() != typeof(ValueObjectMember)))
            {
                code.AppendLine(member.GeneratedCustomClass());
            }

            return code.FormatCode();
        }


        public ValueObjectMember()
        {
            this.TypeName = typeof(object).GetShortAssemblyQualifiedName();
        }

        [JsonIgnore]
        public new Type Type
        {
            get { return null; }
            set
            {
                Console.WriteLine(value);
            }
        }
    }
}