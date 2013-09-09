namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class WebsiteFormElement : FormElement
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
            var path = this.Path;
            if (null != this.CustomField)
                path = string.Format("CustomField('{0}')", this.Path);

            return string.Format("value: {0}, visible :{1}",
                path,
                this.Visible);
        }
    }
}