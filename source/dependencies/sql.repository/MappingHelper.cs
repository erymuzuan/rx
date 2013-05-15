﻿using System;

namespace Bespoke.Sph.SqlRepository
{
    public static class MappingHelper
    {
        public static object MapColumnValue(this CommercialSpace.Domain.Entity item, string column)
        {
            var type = item.GetType().Name;
            switch (type)
            {
                case "Building":
                    var building = (CommercialSpace.Domain.Building)item;
                    if (column == "State")
                        return building.Address.State;
                    break;      
            }
            return DBNull.Value;
        }
    }
}