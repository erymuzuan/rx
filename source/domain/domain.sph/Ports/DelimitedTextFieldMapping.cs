namespace Bespoke.Sph.Domain
{
    public partial class DelimitedTextFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            if (this.IsComplex)
                return new DelimitedTextComplexMember(this);
            return new DelimitedTextSimpleMember(this);
        }
    }
}