namespace Bespoke.Sph.Domain
{
    public partial class ComboBox : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path.ConvertJavascriptObjectToFunction();
            if (null != this.ComboBoxLookup
                && !string.IsNullOrWhiteSpace(this.ComboBoxLookup.Entity)
                && !string.IsNullOrWhiteSpace(this.ComboBoxLookup.ValuePath)
                && !string.IsNullOrWhiteSpace(this.ComboBoxLookup.DisplayPath)
                )
            {
                var lookup = this.ComboBoxLookup;
                var query = string.IsNullOrWhiteSpace(lookup.Query) ?
                    lookup.Entity + "Id gt 0"
                    : lookup.Query.Replace("'", "\\'");

                return string.Format(" visible :{1}, " +
                                     "comboBoxLookupOptions : {{ " +
                                     "value: {0}, " +
                                     "entity : '{2}', " +
                                     "valuePath : '{3}', " +
                                     "displayPath: '{4}', " +
                                     "query :'{5}'" +
                                     "}}",
                                     path,
                                     this.Visible,
                                     lookup.Entity,
                                     lookup.ValuePath,
                                     lookup.DisplayPath,
                                     query);
            }


            return string.Format("value: {0}, visible :{1}",
                path,
                this.Visible);
        }

    }
}