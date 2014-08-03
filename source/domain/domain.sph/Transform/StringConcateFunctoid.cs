using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "String concatenation", FontAwesomeIcon = "sort-numeric-asc", Category = FunctoidCategory.String)]
    public partial class StringConcateFunctoid : Functoid
    {
        public override string GeneratePreCode(FunctoidMap map)
        {
            var code = new StringBuilder();
            code.AppendLine();
            foreach (var arg in this.ArgumentCollection)
            {
                arg.Name = GetRunningNumber().ToString(CultureInfo.InvariantCulture);
                code.AppendLine(arg.Functoid.GeneratePreCode(map));
                code.AppendFormat("               var val{0} = {1};", arg.Name, arg.Functoid.GenerateCode());
            }
            return code.ToString();
        }

        public override string GenerateCode()
        {
            var codes = this.ArgumentCollection.Select(a => "val" + a.Name);
            return string.Join(" + ", codes);
        }
    }
}