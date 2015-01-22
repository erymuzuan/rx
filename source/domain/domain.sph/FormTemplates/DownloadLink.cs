using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof (FormElement))]
    [DesignerMetadata(Name = "Download file", Order = 17d, FontAwesomeIcon = "cloud-download", TypeName = "DownloadLink",
        Description = "Creates a command to download a file for given StoreId")]
    public partial class DownloadLink : FormElement
    {
        public override bool RenderOwnLabel
        {
            get { return true; }
        }
    }
}