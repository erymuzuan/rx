using System;

namespace Bespoke.Sph.SqlRepository
{
    public static class MappingHelper
    {
        public static object MapColumnValue(this CommercialSpace.Domain.Entity item, string column)
        {
            return DBNull.Value;
        }
    }
}