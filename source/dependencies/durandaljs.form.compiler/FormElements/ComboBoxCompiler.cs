﻿using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(ComboBox))]
    public class ComboBoxCompiler : DurandalJsElementCompiler<ComboBox>
    {
      
        public string GetKnockoutBindingExpression()
        {
            var cbb = this.Element;
            var path = cbb.Path.ConvertJavascriptObjectToFunction();
            if (null != cbb.ComboBoxLookup
                && !string.IsNullOrWhiteSpace(cbb.ComboBoxLookup.Entity)
                && !string.IsNullOrWhiteSpace(cbb.ComboBoxLookup.ValuePath)
                && !string.IsNullOrWhiteSpace(cbb.ComboBoxLookup.DisplayPath)
                )
            {
                var lookup = cbb.ComboBoxLookup;
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
                                     cbb.Visible,
                                     lookup.Entity,
                                     lookup.ValuePath,
                                     lookup.DisplayPath,
                                     query);
            }


            return string.Format("value: {0}, visible :{1}, enable :{2}", path, cbb.Visible, cbb.Enable);
        }
    }
}
