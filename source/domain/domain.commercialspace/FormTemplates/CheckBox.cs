namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class CheckBox : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            return string.Format("checked : {0}, visible : {1}",
                this.Path,
                this.Visible);
        }
    }
}