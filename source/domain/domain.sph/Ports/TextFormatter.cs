using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class TextFormatter : DomainObject
    {
        public virtual Task<TextFieldMapping[]> PopulateMappingsAsync()
        {
            throw new System.NotImplementedException();
        }
    }

    public partial class TextFieldMapping : DomainObject { }
    public partial class HtmlTextFormatter : TextFormatter { }
    public partial class JsonTextFormatter : TextFormatter { }
    public partial class XmlTextFormatter : TextFormatter { }
}