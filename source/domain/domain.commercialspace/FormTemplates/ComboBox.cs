using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ComboBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
            {
                IsRequired = this.IsRequired,
                Name = this.Path,
                Listing =string.Join(",", this.ComboBoxItemCollection.Select(c => c.Value).ToArray())
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