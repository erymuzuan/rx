﻿using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchQueryParsers
{
    public class FiltersVisitor : JTokenVisitor<IList<Filter>>
    {
        protected override IList<Filter> Visit(JToken p)
        {
            var filters = from st in p
                          select DynamicVisit(st);
            return filters.SelectMany(x => x).ToList();
        }

        protected override IList<Filter> Visit(JProperty c)
        {
            var list = new List<Filter>();
            if (c.Name == "term")
            {
                var childs = DynamicVisit(new TermJProperty(c));
                list.AddRange(childs);
            }
            if (c.Name == "bool")
            {
                var childs = DynamicVisit(new BoolJProperty(c));
                list.AddRange(childs);
            }
            if (c.Name == "range")
            {
                var childs = DynamicVisit(new RangeJProperty(c));
                list.AddRange(childs);
            }

            return list;
        }

        protected override IList<Filter> Visit(RangeJProperty c)
        {
            var list = new List<Filter>();
            if (c.Value.First is JProperty jp && jp.First is JObject range)
            {
                var lte = range.GetTokenValue<int?>("lte");
                var gte = range.GetTokenValue<int?>("gte");
                var gt = range.GetTokenValue<int?>("gt");
                var lt = range.GetTokenValue<int?>("lt");

                if (null != gt)
                    list.Add(new Filter(jp.Name, Operator.Gt, gt.Value));
                if (null != lt)
                    list.Add(new Filter(jp.Name, Operator.Lt, lt.Value));
                if (null != lte)
                    list.Add(new Filter(jp.Name, Operator.Le, lte.Value));
                if (null != gte)
                    list.Add(new Filter(jp.Name, Operator.Ge, gte.Value));
            }


            return list;
        }

        protected override IList<Filter> Visit(BoolJProperty c)
        {
            var list = new List<Filter>();
            var prop = c.First;

            foreach (var must in prop.SelectToken2("$.must").Concat(prop.SelectToken2("$.filter")))
            {
                var mustFilters = DynamicVisit(must);
                list.AddRange(mustFilters);
            }

            foreach (var mn in prop.SelectToken2("$.must_not"))
            {
                var mustNotFilters = DynamicVisit(mn);
                mustNotFilters.ToList().ForEach(x => x.Operator = x.Operator.Invert());
                list.AddRange(mustNotFilters);
            }
            if(prop.SelectToken("should") is JArray sjp)
                list.AddRange(DynamicVisit(new ShouldJProperty(sjp)));
          

            // TODO : when should is just a property not an array

            return list;
        }

        protected override IList<Filter> Visit(ShouldJProperty should)
        {
            var or = new CompoundOrFilter();
            foreach (var node in should.Children())
            {
                or.Filters.AddRange(DynamicVisit(node));
            }
            return new List<Filter> { or };
        }

        protected override IList<Filter> Visit(JArray c)
        {
            var list = c.Select(DynamicVisit).SelectMany(x => x).ToList();

            return list;
        }

        protected override IList<Filter> Visit(TermJProperty term)
        {
            var list = new List<Filter>();
            if (term.First?.First is JProperty field)
            {
                list.Add(new Filter(field.Name, Operator.Eq, field.Value));
            }
            return list;
        }
    }
}