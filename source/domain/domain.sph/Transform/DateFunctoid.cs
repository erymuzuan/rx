using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Date Parsing", BootstrapIcon = "calendar")]
    public partial class DateFunctoid : Functoid
    {
        public override string GenerateCode()
        {
            if (string.IsNullOrWhiteSpace(this.DateTimeStyles))
                this.DateTimeStyles = "None";

            return string.Format("DateTime.ParseExact(item.{0}, \"{1}\", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.{2})", this.SourceField, this.Format, this.DateTimeStyles);
        }
    }
}