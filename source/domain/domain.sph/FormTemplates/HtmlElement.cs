namespace Bespoke.Sph.Domain
{
    public partial class HtmlElement : FormElement
    {
        public override bool IsPathIsRequired
        {
            get { return false; }
        }
    }
}