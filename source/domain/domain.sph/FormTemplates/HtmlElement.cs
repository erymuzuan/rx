namespace Bespoke.Sph.Domain
{
    public partial class HtmlElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return null;
        }

        public override bool IsPathIsRequired
        {
            get { return false; }
        }
    }
}