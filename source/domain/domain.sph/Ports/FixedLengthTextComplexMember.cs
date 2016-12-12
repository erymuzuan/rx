using Humanizer;

namespace Bespoke.Sph.Domain
{
    public class FixedLengthTextComplexMember : ComplexMember
    {
        private readonly FixedLengthTextFieldMapping m_fieldMapping;

        public FixedLengthTextComplexMember(FixedLengthTextFieldMapping fieldMapping)
        {
            m_fieldMapping = fieldMapping;
            this.Name = fieldMapping.Name;
            this.TypeName = fieldMapping.TypeName;
            this.AllowMultiple = true;
            this.WebId = fieldMapping.WebId;
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