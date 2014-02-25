namespace Bespoke.Sph.Domain
{
    public partial class DateTimePicker : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            if (this.IsCompact)
                return string.Format("kendoDate: {0}, visible :{1}, enable :{2}",
                    this.Path,
                    this.Visible, this.Enable);
            return string.Format("kendoDate: {0}, enable :{1}", this.Path, this.Enable);
        }

    }
}