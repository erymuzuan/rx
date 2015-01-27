﻿using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(ChildEntityListView))]
    public class ChildEntityListViewCompiler : DurandalJsElementCompiler<ChildEntityListView>
    {
        public string Expression
        {
            get
            {
                return this.Element.Path.ConvertJavascriptObjectToFunction();
            }
        }
    }
}