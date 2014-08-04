using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export(DESIGNER_CONTRACT, typeof(Functoid))]
    [FunctoidDesignerMetadata(Name = "Parse boolean", FontAwesomeIcon = "check-circle", Category = FunctoidCategory.Common)]
    public partial class BooleanFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg{Name = "value", Type = typeof(string)});

            return base.Initialize();
        }

        public override string GenerateCode()
        {
            return string.Format("bool.Parse(item.{0})", this.SourceField);
        }
    }
}