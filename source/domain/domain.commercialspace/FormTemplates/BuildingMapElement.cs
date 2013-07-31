using System;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class BuildingMapElement : FormElement
    {
        public override CustomField GenerateCustomField()
        {
            throw new Exception("Custom field is not supported for Building Template.Please provide path");
        }
    }
}