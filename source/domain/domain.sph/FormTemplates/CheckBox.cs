namespace Bespoke.Sph.Domain
{
    public partial class CheckBox : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            return string.Format("checked : {0}, enable :{1}", this.Path, this.Enable);
        }

    }
}