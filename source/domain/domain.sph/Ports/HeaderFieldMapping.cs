using System.Text;

namespace Bespoke.Sph.Domain
{
    public class HeaderFieldMember : SimpleMember
    {
        public override string GeneratedCode(string padding = "      ")
        {
            var nullabe = this.Type == typeof(string) ? "" : "?";
         
            return $"[FieldHidden] public {Type.ToCSharp()}{nullabe} {Name};";
        }
    }
    public partial class HeaderFieldMapping : TextFieldMapping
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