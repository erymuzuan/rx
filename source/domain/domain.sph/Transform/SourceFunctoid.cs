namespace Bespoke.Sph.Domain
{
    public partial class SourceFunctoid : Functoid
    {
        public override string GenerateAssignmentCode()
        {
            return "item." + this.Field;
        }
    }
}