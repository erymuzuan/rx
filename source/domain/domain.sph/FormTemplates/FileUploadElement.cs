using System;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class FileUploadElement : FormElement
    {
        public override string GetKnockoutBindingExpression()
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