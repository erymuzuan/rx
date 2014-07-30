using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScriptFunctoid : Functoid
    {
        public override string GeneratePreCode(FunctoidMap map)
        {
            var block = this.Expression;
            if (!block.EndsWith(";")) return string.Empty;
            
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLine();
            code.AppendLinf("               Func<{{SOURCE_TYPE}}, {1}> {0} = d =>", this.Name, map.DestinationType.FullName);
            code.AppendLine("                                           {");
            code.AppendLine("                                               " + this.Expression);
            code.AppendLine("                                           };");
            return code.ToString();
        }

        public override string GenerateCode()
        {
            if(string.IsNullOrWhiteSpace(this.Name))throw new InvalidOperationException("Name cannot be empty");
            var block = this.Expression;
            if (!block.EndsWith(";")) return this.Expression;

            return string.Format("{0}(item)", this.Name);
        }

        public string Name{ get; set; }
    }
}