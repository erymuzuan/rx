using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Now", BootstrapIcon = "calendar")]
    public class AddDaysFunctoid : Functoid
    {
        public AddDaysFunctoid()
        {
            this.ArgumentCollection.Add(new FunctoidArg { Name = "date", Type = typeof(DateTime) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(double) });
        }

        private int m_number;

        public override string GeneratePreCode(FunctoidMap map)
        {
            var date = this.ArgumentCollection.Single(x => x.Name == "date").Functoid;
            var value = this.ArgumentCollection.Single(x => x.Name == "value").Functoid;

            var code = new StringBuilder();
            code.AppendLine();
            m_number = Functoid.GetRunningNumber();

            code.AppendLine(date.GeneratePreCode(map));
            code.AppendLinf("               var date{0} = {1};", m_number, date.GenerateCode());
            code.AppendLine(value.GeneratePreCode(map));
            code.AppendFormat("               var value{0} = {1};", m_number, value.GenerateCode());
            return code.ToString();
        }

        public override string GenerateCode()
        {
            return string.Format("date{0}.AddDays(value{0})", m_number);
        }
    }
}