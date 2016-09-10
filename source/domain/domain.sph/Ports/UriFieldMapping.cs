namespace Bespoke.Sph.Domain
{
    public partial class UriFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            return new HeaderFieldMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                Type = this.Type,
                IsNullable = this.IsNullable
            };
        }
    }
}