using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class TextBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
            {
                Name = this.Path,
                Type = typeof(string).Name,
                IsRequired = this.IsRequired,
                MaxLength = this.MaxLength,
                MinLength = this.MinLength
            };
        }

        public override string GenerateMarkup()
        {
            var element = new StringBuilder();
            element.AppendFormat("<input type='text' data-bind='value:{0}' name='{1}'></input>", this.Path, this.Name);
            return element.ToString();
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