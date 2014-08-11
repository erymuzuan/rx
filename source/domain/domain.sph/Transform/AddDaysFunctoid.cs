using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "AddDays", BootstrapIcon = "calendar", Category = FunctoidCategory.DATE)]
    public class AddDaysFunctoid : Functoid
    {
        public override async Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var errors =(await base.ValidateAsync()).ToList();
            if(this["date"] == null)
                errors.Add("date","date argument is missing", this.WebId);
            if(this["value"] == null)
                errors.Add("value", "value argument is missing", this.WebId);
            return errors;
        }

        public override bool Initialize()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "date", Type = typeof(DateTime) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(double) });
            return base.Initialize();

        }


        public override string GenerateStatementCode()
        {
            var date = this.ArgumentCollection.Single(x => x.Name == "date").GetFunctoid(this.TransformDefinition);
            var value = this.ArgumentCollection.Single(x => x.Name == "value").GetFunctoid(this.TransformDefinition);

            var code = new StringBuilder();
            code.AppendLine();

            code.AppendLine(date.GenerateStatementCode());
            code.AppendLinf("               var date{0} = {1};", this.Index, date.GenerateAssignmentCode());
            code.AppendLine(value.GenerateStatementCode());
            code.AppendFormat("               var value{0} = {1};", this.Index, value.GenerateAssignmentCode());
            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("date{0}.AddDays(value{0})", this.Index);
        }
    }
}