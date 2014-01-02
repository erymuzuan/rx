namespace Bespoke.Sph.Domain
{
    public partial class WebsiteFormElement : FormElement
    {
      

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            return string.Format("value: {0}, visible :{1}",
                path,
                this.Visible);
        }
    }
}