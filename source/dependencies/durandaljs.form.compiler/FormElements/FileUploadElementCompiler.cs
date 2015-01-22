using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(FileUploadElement))]
    public class FileUploadElementCompiler : DurandalJsElementCompiler<FileUploadElement>
    {


        public string GetKnockoutBindingExpression()
        {
            var upload = this.Element;
            var path = upload.Path;
            if (string.IsNullOrWhiteSpace(upload.AllowedExtensions))
                return string.Format("kendoUpload : {0}, visible :{1}", path, upload.Visible);

            var extensions =
                    string.Join(",",
                        upload.AllowedExtensions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(c => "'" + c + "'"));

            return string.Format("kendoUpload : {{value:{0},extensions:[{2}]}}, visible :{1}",
                path,
                upload.Visible, extensions);
        }
    }
}