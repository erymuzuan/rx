using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "AddDays", BootstrapIcon = "calendar", Category = FunctoidCategory.Date)]
    public class AddDaysFunctoid : Functoid
    {
        private int m_number;
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


        public override string GeneratePreCode()
        {
            var date = this.ArgumentCollection.Single(x => x.Name == "date").GetFunctoid(this.TransformDefinition);
            var value = this.ArgumentCollection.Single(x => x.Name == "value").GetFunctoid(this.TransformDefinition);

            var code = new StringBuilder();
            code.AppendLine();
            m_number = GetRunningNumber();

            code.AppendLine(date.GeneratePreCode());
            code.AppendLinf("               var date{0} = {1};", m_number, date.GenerateCode());
            code.AppendLine(value.GeneratePreCode());
            code.AppendFormat("               var value{0} = {1};", m_number, value.GenerateCode());
            return code.ToString();
        }

        public override string GenerateCode()
        {
            return string.Format("date{0}.AddDays(value{0})", m_number);
        }
    }
}