﻿using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticsearchRepository.Extensions
{
    public static class SortExtension
    {

        public static string GenerateQuery(this Sort sort)
        {
            var direction = sort.Direction == SortDirection.Asc ? "asc" : "desc";
            return $@"{{""{sort.Path}"" : {{""order"": ""{direction}""}}}}";
        }

        
    }
}