using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS)]
    public class DurandalJsFormCompiler : FormCompiler
    {

    }
}
