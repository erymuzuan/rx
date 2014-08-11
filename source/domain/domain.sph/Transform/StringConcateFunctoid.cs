using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "String concatenation", FontAwesomeIcon = "plus-circle", Category = FunctoidCategory.String)]
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

        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            foreach (var arg in this.ArgumentCollection.Where(x => !string.IsNullOrWhiteSpace(x.Functoid)))
            {
                var ftd = arg.GetFunctoid(this.TransformDefinition);
                arg.Name = GetRunningNumber().ToString(CultureInfo.InvariantCulture);
                code.AppendLine(ftd.GenerateStatementCode());
                code.AppendFormat("               var val{0} = {1};", arg.Name, ftd.GenerateAssignmentCode());
            }
            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            var codes = this.ArgumentCollection.Where(x => !string.IsNullOrWhiteSpace(x.Functoid)).Select(a => "val" + a.Name);
            return string.Join(" + ", codes);
        }

        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors = new List<ValidationError>();
            var vfTasks = from a in this.ArgumentCollection
                          where !string.IsNullOrWhiteSpace(a.Functoid)
                          let fnt = a.GetFunctoid(this.TransformDefinition)
                          select fnt.ValidateAsync();

            var vf = (await Task.WhenAll(vfTasks)).SelectMany(x => x.ToArray());
            errors.AddRange(vf);

            return errors;
        }
    }
}