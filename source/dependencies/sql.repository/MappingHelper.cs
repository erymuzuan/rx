using System;
using Bespoke.Station.Domain;

namespace Bespoke.Station.SqlRepository
{
    public static class MappingHelper
    {
        public static object MapColumnValue(this Entity item, string column)
        {
            var type = item.GetType().Name;
            switch (type)
            {
                case "Sale":
                    if (column == "ProductProductId")
                        return ((Domain.Sale)item).Product.ProductId;
                    if (column == "ProductCode")
                        return ((Domain.Sale)item).Product.Code;
                    break;
                case "Employee":
                    if (column == "EmploymentDateLeft")
                    {
                        var dl = ((Domain.Employee)item).Employment.DateLeft;
                        if (null == dl) return DBNull.Value;
                        return dl;
                    }
                    break;
            }

            return DBNull.Value;
        }
    }
}