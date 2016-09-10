namespace Bespoke.Sph.Domain
{
    public class HeaderFieldMember : SimpleMember
    {
        public HeaderFieldMember(TextFieldMapping field)
        {
            Name = field.Name;
            TypeName = field.TypeName;
            IsNullable = this.IsNullable;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            var nullabe = this.Type == typeof(string) ? "" : "?";
            return $"[FieldHidden] public {Type.ToCSharp()}{nullabe} {Name};";
        }
    }
}