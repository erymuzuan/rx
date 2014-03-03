namespace Bespoke.Sph.Domain
{
    public partial class EmailFormElement : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path.ConvertJavascriptObjectToFunction();
         
            return string.Format("value: {0}, visible :{1}, enable: {2}",
                path,
                this.Visible,
                this.Enable ?? "true");
        }
    }
}