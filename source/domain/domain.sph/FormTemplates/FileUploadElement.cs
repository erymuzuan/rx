namespace Bespoke.Sph.Domain
{
    public partial class FileUploadElement : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;
            return string.Format("kendoUpload : {0}, visible :{1}",
                path,
                this.Visible);
        }

    }
}