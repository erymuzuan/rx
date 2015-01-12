using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Sph.Domain;

namespace form.renderer.html
{
    [Export(typeof(IElementRenderer<Button>))]
    [FormMetadata(Name = "Html")]
    public class ButtonRenderer
    {
        public string Render(Button button)
        {
            var code = new HtmlBuilder();
            code.AppendElement("div")
                .AddClass("form-group");

            return code.ToString();
        }
    }

    public class HtmlBuilder
    {

        public HtmlBuilder AppendElement(string element)
        {
            return this;
        }
        public HtmlBuilder AddClass(string element)
        {
            return this;
        }
    }

}