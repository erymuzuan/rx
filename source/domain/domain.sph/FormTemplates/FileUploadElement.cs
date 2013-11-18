namespace Bespoke.Sph.Domain
{
    public partial class FileUploadElement : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            if (null != this.CustomField)
                path = string.Format("CustomField('{0}')", this.Path);
            return string.Format("kendoUpload : {0}, visible :{1}",
                path,
                this.Visible);
        }

        public override CustomField GenerateCustomField()
        {
            return null;
        }
    }
}