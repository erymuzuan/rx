using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexMember
    {
        public override string GetEsMapping()
        {
            var map = new StringBuilder();
            map.AppendLine($"    \"{Name}\":{{");

            map.AppendLine("        \"type\":  \"object\",");
            map.AppendLine("        \"properties\":{");

            var memberMappings = string.Join(",\r\n", this.MemberCollection.Select(d => d.GetEsMapping()));
            map.AppendLine(memberMappings);

            map.AppendLine("        }");
            map.AppendLine("    }");
            return map.ToString();
        }
    }
}