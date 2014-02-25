namespace Bespoke.Sph.Domain
{
    public partial class TextAreaElement : FormElement
    {
       
       

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;

            return string.Format("{2}: {0}, visible :{1}, enable :{3}",
                path,
                this.Visible,
                this.IsHtml ? "kendoEditor" : "value",
                this.Enable);
        }
    }
}

