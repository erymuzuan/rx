using System;
using Bespoke.Sph.Domain;

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
                    break;
                case "Contract":
                    break;
                case "RentalApplication":
                    break;      
            }
            return DBNull.Value;
        }
    }
}