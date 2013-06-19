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
                    var building = (Building)item;
                    if (column == "State")
                        return building.Address.State;
                    break;
                case "Contract":
                    var contract = (Contract)item;
                    if (column == "TenantName")
                        return contract.Tenant.Name;
                    if (column == "TenantRegistrationNo")
                        return contract.Tenant.RegistrationNo;
                    if (column == "TenantIdSsmNo")
                        return contract.Tenant.IdSsmNo ?? "";
                    if (column == "CommercialSpaceId")
                        return contract.CommercialSpace.CommercialSpaceId;
                    if (column == "CommercialSpaceRegistrationNo")
                        return contract.CommercialSpace.RegistrationNo;
                    break;
                case "RentalApplication":
                    var app = (RentalApplication)item;
                    if (column == "ContactName")return app.Contact.Name;
                    if (column == "ContactIcNo")return app.Contact.IcNo;
                    break;      
            }
            return DBNull.Value;
        }
    }
}