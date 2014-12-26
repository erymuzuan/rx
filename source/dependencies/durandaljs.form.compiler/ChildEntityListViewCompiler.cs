using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof (ChildEntityListView))]
    public class ChildEntityListViewCompiler : DurandalJsElementCompiler<ChildEntityListView>
    {


    }
}