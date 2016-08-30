namespace Bespoke.Sph.Domain
{
    public class DelimitedTextSimpleMember : SimpleMember
    {
        private readonly DelimitedTextFieldMapping m_fieldMapping;


        public DelimitedTextSimpleMember(DelimitedTextFieldMapping fieldMapping)
        {
            m_fieldMapping = fieldMapping;
            this.Name = fieldMapping.Path;
            this.TypeName = fieldMapping.TypeName;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            var code = base.GeneratedCode(padding);
            if (string.IsNullOrWhiteSpace(this.m_fieldMapping.Converter))
                return code;
            return $@"//[Converter(""{m_fieldMapping.Converter}"")]\r\n" + code;
        }
    }

    public partial class DelimitedTextFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            if (!this.IsComplex && !string.IsNullOrWhiteSpace(this.Converter))
            {
                return new DelimitedTextSimpleMember(this);
            }
            return base.GenerateMember();
        }
    }
}