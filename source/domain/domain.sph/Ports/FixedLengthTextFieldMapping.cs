namespace Bespoke.Sph.Domain
{
    public partial class FixedLengthTextFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            if (this.IsComplex)
                return new FixedLengthTextComplexMember(this);
            return new FixedLengthTextSimpleMember(this);
        }
    }
}