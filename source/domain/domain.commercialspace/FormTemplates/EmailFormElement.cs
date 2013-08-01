namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class EmailFormElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
            {
                Name = this.Path,
                Type = typeof(string).Name,
                IsRequired = this.IsRequired
            };
        }

        public override string GetKnockoutBindingExpression()
        {
            return string.Format("value: {0}, visible :{1}",
                this.Path,
                this.Visible);
        }
    }
}