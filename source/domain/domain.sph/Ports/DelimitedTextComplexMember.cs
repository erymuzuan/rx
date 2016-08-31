using Humanizer;

namespace Bespoke.Sph.Domain
{
    public class DelimitedTextComplexMember : ComplexMember
    {
        private readonly DelimitedTextFieldMapping m_fieldMapping;

        public DelimitedTextComplexMember(DelimitedTextFieldMapping fieldMapping)
        {
            m_fieldMapping = fieldMapping;
            this.Name = fieldMapping.Path;
            this.TypeName = fieldMapping.TypeName;
        }

        public override string GeneratedCode(string padding = "      ")
        {
            var memberName = TypeName.Pluralize().ToCamelCase();
            return $@"

        [FieldHidden]
        private readonly IList<{TypeName}> m_{memberName} = new List<{TypeName}>();
        public IList<{TypeName}> {Name} => m_{memberName};";
        }
    }
}