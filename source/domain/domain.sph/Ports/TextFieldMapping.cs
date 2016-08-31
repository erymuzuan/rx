using System.Diagnostics;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Name}({SampleValue}), TypeName= {TypeName}")]
    public partial class TextFieldMapping : DomainObject
    {
        public virtual Member GenerateMember()
        {
            var simple = new SimpleMember
            {
                Name = this.Name,
                TypeName = this.TypeName,
                AllowMultiple = false,
                IsNullable =  this.IsNullable,
                IsNotIndexed = true
                
            };
            if (!this.IsComplex) return simple;

            var complex = new ComplexMember
            {
                Name = this.Name,
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