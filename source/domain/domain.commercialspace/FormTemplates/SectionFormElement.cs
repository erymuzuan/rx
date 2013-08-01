namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class SectionFormElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return null;
        }

        public override string GetKnockoutBindingExpression()
        {
            if (!string.IsNullOrWhiteSpace(this.Visible))
                return string.Format("visible : {0}", this.Visible);

            return null;
        }
    }
}