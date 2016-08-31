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
            var kind = "";
            /*
    None,
    Date,
    Boolean,
    Byte,
    Int16,
    Int32,
    Int64,
    Decimal,
    Double,
    PercentDouble,
    Single,
    SByte,
    UInt16,
    UInt32,
    UInt64,
    DateMultiFormat,
    Char,
    Guid,*/
            switch (this.TypeName)
            {
                case "System.DateTime, mscorlib":
                    kind = "Date"; break;
                case "System.Int32, mscorlib":
                    kind = "Int"; break;
                case "System.Decimal, mscorlib":
                    kind = "Decimal"; break;
                case "System.Boolean, mscorlib":
                    kind = "Boolean"; break;
                default:
                    kind = "None";
                    break;
            }
            var converter = string.IsNullOrWhiteSpace(this.m_fieldMapping.Converter)
                ? ""
                : $@"[FieldConverter(ConverterKind.{kind}, ""{m_fieldMapping.Converter}"")]";

            return $@"
{padding}{converter}
{padding}[JsonIgnore]
{padding}public {Type.ToCSharp()} {Name}Raw; 
{padding}public {Type.ToCSharp()} {Name} => {Name}Raw;";
        }
    }

    public partial class DelimitedTextFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember() => IsComplex ? base.GenerateMember() : new DelimitedTextSimpleMember(this);
    }
}