namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class NumberTextBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            var cf = new CustomField
            {
                Name = this.Path,
                Type = typeof(int).Name
            };
            this.CustomField = cf;
            return cf;
        }
    }
}