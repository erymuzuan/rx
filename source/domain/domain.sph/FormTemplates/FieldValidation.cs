using System;
using System.Linq.Expressions;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class FieldValidation : DomainObject
    {
        public string GetHtmlAttributes()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(this.Mode))
                sb.AppendFormat(" data-rule-{0}=\"true\" ", this.Mode.ToLowerInvariant());
            if (this.IsRequired)
                sb.AppendLine(" data-rule-required=\"true\" ");

            this.GetValidationAttribute(sb, x => x.Message);
            this.GetValidationAttribute(sb, x => x.MaxLength);
            this.GetValidationAttribute(sb, x => x.MinLength);
            this.GetValidationAttribute(sb, x => x.Min);
            this.GetValidationAttribute(sb, x => x.Max);


            return sb.ToString();
        }


        private void GetValidationAttribute<T>(StringBuilder sb, Expression<Func<FieldValidation, T>> field)
        {
            var func = field.Compile();
            var val = func(this);
            var stringVal = string.Format("{0}", val);
            if (string.IsNullOrWhiteSpace(stringVal))
                return;

            dynamic fdyn = field.Body;
            string fieldName = fdyn.Member.Name;
            var attribute = string.Format(" data-rule-{0}=\"{1}\"", fieldName.ToLowerInvariant(), val);
            sb.AppendLine(attribute);

        }

    }
}
