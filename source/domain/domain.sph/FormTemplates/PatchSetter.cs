namespace Bespoke.Sph.Domain
{
    public partial class PatchSetter : DomainObject
    {
        public override string ToString()
        {
            return this.Path;
        }
    }
}