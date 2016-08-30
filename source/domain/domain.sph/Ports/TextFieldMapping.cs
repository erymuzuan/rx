using System.Diagnostics;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Path}({SampleValue}), TypeName= {TypeName}")]
    public partial class TextFieldMapping : DomainObject
    {
        public virtual Member GenerateMember()
        {
            var simple = new SimpleMember
            {
                Name = this.Path,
                TypeName = this.TypeName,
                AllowMultiple = false
            };
            if (!this.IsComplex) return simple;

            var complex = new ComplexMember
            {
                Name = this.Path,
                TypeName = this.TypeName,
                AllowMultiple = this.AllowMultiple
            };
            var children = from f in this.FieldMappingCollection
                           select f.GenerateMember();
            complex.MemberCollection.AddRange(children);
            return complex;
        }
    }
}