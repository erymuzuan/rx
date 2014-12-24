using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Upload file",Order = 16d, FontAwesomeIcon = "cloud-upload",TypeName = "FileUploadElement", Description = "Creates an input for file upload")]
    public partial class FileUploadElement : FormElement
    {
        public  string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            if (string.IsNullOrWhiteSpace(this.AllowedExtensions))
                return string.Format("kendoUpload : {0}, visible :{1}", path, this.Visible);

            var extensions =
                    string.Join(",",
                        this.AllowedExtensions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => "'" + c + "'"));

            return string.Format("kendoUpload : {{value:{0},extensions:[{2}]}}, visible :{1}",
                path,
                this.Visible, extensions);
        }

    }
}