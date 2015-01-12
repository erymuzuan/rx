using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export(DESIGNER_CONTRACT, typeof(Functoid))]
    [DesignerMetadata(Name = "Parse boolean", FontAwesomeIcon = "check-circle", Category = FunctoidCategory.COMMON)]
    public partial class ParseBooleanFunctoid : Functoid
    {
        public override bool Initialize()
        {
            this.ArgumentCollection.Clear();
            this.ArgumentCollection.Add(new FunctoidArg { Name = "value", Type = typeof(string) });

            return base.Initialize();
        }

        public override string GenerateAssignmentCode()
        {
            return string.Format("bool.Parse(item.{0})", this.SourceField);
        }
    }
}