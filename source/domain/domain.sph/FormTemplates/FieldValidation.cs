using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class FieldValidation : DomainObject
    {
        public string GetHtmlAttributes()
        {
            var sb = new StringBuilder();
            if (this.IsRequired)
                sb.AppendLine(" data-rule-required=\"true\" ");
            if (!string.IsNullOrWhiteSpace(this.Mode))
                sb.AppendFormat(" data-rule-{0}=\"true\" ", this.Mode.ToLowerInvariant());

            if (!string.IsNullOrWhiteSpace(this.Message))
                sb.AppendFormat(" data-rule-message=\"{0}\" ", this.Message);

            if (this.MinLength.HasValue)
                sb.AppendFormat(" data-rule-minlength=\"{0}\" ", this.MinLength);
            if (this.MaxLength.HasValue)
                sb.AppendFormat(" data-rule-maxlength=\"{0}\" ", this.MaxLength);
            if (this.Min.HasValue)
                sb.AppendFormat(" data-rule-min=\"{0}\" ", this.Min);
            if (this.Max.HasValue)
                sb.AppendFormat(" data-rule-max=\"{0}\" ", this.Max);


            return sb.ToString();
        }
    }
}
