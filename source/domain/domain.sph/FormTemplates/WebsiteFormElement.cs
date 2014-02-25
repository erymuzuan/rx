namespace Bespoke.Sph.Domain
{
    public partial class WebsiteFormElement : FormElement
    {
      

        public override string GetKnockoutBindingExpression()
        {
            return string.Format("value: {0}, visible :{1}, enable: {2}",
                this.Path,
                this.Visible,
                this.Enable);
        }
    }
}