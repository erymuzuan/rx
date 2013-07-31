using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class ComboBox : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            throw new Exception("Custom field is not supported for Building Template.Please provide path");
        }
    }
}