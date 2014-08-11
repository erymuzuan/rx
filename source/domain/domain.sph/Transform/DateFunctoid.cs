using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [DesignerMetadata(Name = "Date Parsing", BootstrapIcon = "calendar")]
    public partial class DateFunctoid : Functoid
    {

        public override sealed bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(string) });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "format", Type = typeof(string), IsOptional = true });
            this.ArgumentCollection.Add(new FunctoidArg { Name = "styles", Type = typeof(string), IsOptional = true });
            return base.Initialize();
        }

        private int m_number;
        public override string GenerateStatementCode()
        {
            m_number = GetRunningNumber();
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLinf("               var value{0} = {1};", m_number, this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());
            code.AppendLinf("               var format{0} = {1};", m_number, this["format"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());
            var style = this["styles"].GetFunctoid(this.TransformDefinition);
            if (null != style)
                code.AppendFormat("               var style{0} = {1};", m_number, style.GenerateAssignmentCode());
            else
                code.AppendFormat("               var style{0} = System.Globalization.DateTimeStyles.None;", m_number);


            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("DateTime.ParseExact(value{0}, format{0}, System.Globalization.CultureInfo.InvariantCulture, style{0})", m_number);
        }
    }
}