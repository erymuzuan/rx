using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FunctoidDesigner", typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Date Parsing", BootstrapIcon = "calendar")]
    public partial class DateFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg{Name = "value", Type = typeof(string)});
            this.ArgumentCollection.Add(new FunctoidArg{Name = "format", Type = typeof(string), IsOptional = true});
            this.ArgumentCollection.Add(new FunctoidArg{Name = "styles", Type = typeof(string), IsOptional = true});
            return base.Initialize();
        }

        public override string GenerateCode()
        {
            return string.Format("DateTime.ParseExact(item.{0}, \"{1}\", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.{2})", this["val"], this["format"], this["styles"]);
        }
    }
}