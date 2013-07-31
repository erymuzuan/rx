using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class AddressElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
           throw new Exception("No Custom Field Element");
        }
    }
}
