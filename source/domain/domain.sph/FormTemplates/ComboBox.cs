using System;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class ComboBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return new CustomField
            {
                IsRequired = this.IsRequired,
                Name = this.Path,
                Listing = string.Join(",", this.ComboBoxItemCollection.Select(c => c.Value).ToArray())
            };
        }


        public override string GetKnockoutBindingExpression()
        {
            if (null != this.ComboBoxLookup 
                && !string.IsNullOrWhiteSpace(this.ComboBoxLookup.Entity))
            {
                var lookup = this.ComboBoxLookup;
                var query = lookup.Query.Replace("'", "\\'");
                return string.Format(" visible :{1}, " +
                                     "comboBoxLookupOptions : {{ " +
                                     "value: {0}, " +
                                     "entity : '{2}', " +
                                     "valuePath : '{3}', " +
                                     "displayPath: '{4}', " +
                                     "query :'{5}'" +
                                     "}}", 
                                     this.Path, this.Visible,
                                     lookup.Entity,
                                     lookup.ValuePath,
                                     lookup.DisplayPath,
                                     query);
            }


            var path = this.Path;
            if (null != this.CustomField)
                path = string.Format("CustomField('{0}')", this.Path);
            return string.Format("value: {0}, visible :{1}",
                path,
                this.Visible);
        }

    }
}