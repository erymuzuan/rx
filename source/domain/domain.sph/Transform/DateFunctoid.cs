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
        public override string GenerateStatementCode()
        {
            var code = new StringBuilder();
            code.AppendLine();
            code.AppendLinf("               var value{0} = {1};", this.Index, this["value"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());
            code.AppendLinf("               var format{0} = {1};", this.Index, this["format"].GetFunctoid(this.TransformDefinition).GenerateAssignmentCode());
            var style = this["styles"].GetFunctoid(this.TransformDefinition);
            if (null != style)
                code.AppendFormat("               var style{0} = {1};", this.Index, style.GenerateAssignmentCode());
            else
                code.AppendFormat("               var style{0} = System.Globalization.DateTimeStyles.None;", this.Index);


            return code.ToString();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("DateTime.ParseExact(value{0}, format{0}, System.Globalization.CultureInfo.InvariantCulture, style{0})", this.Index);
        }
    }
}