using System;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.SqlRepository
{
    public static class MappingHelper
    {
        public static object MapColumnValue(this Entity item, string column)
        {
            var type = item.GetType().Name;
            switch (type)
            {
                case "Building":
                    var building = (SphCommercialSpaces.Domain.Building)item;
                    if (column == "State")
                        return building.Address.State;
                    break;     
                case "RentalApplication":
                    var app = (SphCommercialSpaces.Domain.RentalApplication)item;
                    if (column == "ContactName")return app.Contact.Name;
                    if (column == "ContactIcNo")return app.Contact.IcNo;
                    break;      
            }
            return DBNull.Value;
        }
    }
}