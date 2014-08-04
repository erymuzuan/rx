using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "String concatenation", FontAwesomeIcon = "plus-circle", Category = FunctoidCategory.String)]
    public partial class StringConcateFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            var args = from i in Enumerable.Range(1, 10)
                select new FunctoidArg
                {
                    Name = i.ToString(CultureInfo.InvariantCulture),
                    Type = typeof(object)
                };
            this.ArgumentCollection.AddRange(args);

            return base.Initialize();
        }

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