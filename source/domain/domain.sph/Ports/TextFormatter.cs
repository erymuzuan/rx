namespace Bespoke.Sph.Domain
{
    public partial class TextFormatter : DomainObject{}
    public partial class FixedLengthTextFormatter : TextFormatter{}
    public partial class DelimitedTextFormatter : TextFormatter{}
    public partial class JsonTextFormatter : TextFormatter{}
    public partial class XmlTextFormatter : TextFormatter{}
    public partial class FlatFileDetailTag : DomainObject{}
}