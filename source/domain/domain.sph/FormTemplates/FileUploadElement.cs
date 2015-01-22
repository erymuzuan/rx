using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Upload file",Order = 16d, FontAwesomeIcon = "cloud-upload",TypeName = "FileUploadElement", Description = "Creates an input for file upload")]
    public partial class FileUploadElement : FormElement
    {



    }
}