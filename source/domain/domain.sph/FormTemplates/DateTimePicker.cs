namespace Bespoke.Sph.Domain
{
    public partial class DateTimePicker : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            return string.Format("kendoDateTime: {0}, visible :{1}",
                path,
                this.Visible);
        }

    }
}