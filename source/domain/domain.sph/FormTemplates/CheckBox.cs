namespace Bespoke.Sph.Domain
{
    public partial class CheckBox : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            return string.Format("checked : {0}, visible : {1}",
                path,
                this.Visible);
        }

    }
}