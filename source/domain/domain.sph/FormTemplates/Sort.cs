namespace Bespoke.Sph.Domain
{
    public partial class Sort : DomainObject
    {
        public override string ToString()
        {
            return $"{Path} {Direction}";
        }
    }
}