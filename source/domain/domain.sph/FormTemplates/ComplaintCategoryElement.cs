namespace Bespoke.Sph.Domain
{
    public partial class ComplaintCategoryElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            return null;
        }

        public override string GetKnockoutBindingExpression()
        {
            return string.Format("value:Category, options:$root.categoryOptions");
        }

        public string GetSubCategoryKnockoutBindingExpression()
        {
            return string.Format("value:SubCategory, options:$root.subCategoryOptions");
        }
    }
}