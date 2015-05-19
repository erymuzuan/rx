﻿using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Select List", TypeName = "ComboBox", Order = 2d, FontAwesomeIcon = "chevron-down", Description = "Creates an input with list of options")]
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
                var query = string.IsNullOrWhiteSpace(lookup.Query) ? "'Id ne \\'0\\''"
                    : "'" + lookup.Query.Replace("'", "\\'") + "'";
                if (lookup.IsComputedQuery)
                    query = "ko.computed(function(){ return " + lookup.Query + ";})";

                return string.Format(" visible :{1}, " +
                                     "comboBoxLookupOptions : {{ " +
                                     "value: {0}, " +
                                     "entity : '{2}', " +
                                     "valuePath : '{3}', " +
                                     "displayPath: '{4}', " +
                                     "query :{5}" +
                                     "}}",
                                     path,
                                     this.Visible,
                                     lookup.Entity,
                                     lookup.ValuePath,
                                     lookup.DisplayPath,
                                     query);
            }


            return this.IsCompact ?
                $"value: {path}, visible :{this.Visible}, enable :{this.Enable}"
                : $"value: {path}, enable :{this.Enable}";
        }

    }
}