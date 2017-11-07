﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace elasticsearc.repository.test
{
    [Trait("Category", "Query endpoints")]
    [Collection("Endpoint")]
    public class QueryTest
    {

        [Fact]
        [Trait("Query", "Elasticsearch")]
        public async Task CompileQueryFieldsAndFilter()
        {
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = "Patient",
                WebId = "all-born-in-60s"
            };
            var sixty = new ConstantField { Type = typeof(DateTime), Value = "1960-01-01" };
            var sixtyNine = new ConstantField { Type = typeof(DateTime), Value = "1969-12-31" };
            query.FilterCollection.Add(new Filter
            {
                Field = sixty,
                Operator = Operator.Ge,
                Term = "Dob"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = sixtyNine,
                Operator = Operator.Le,
                Term = "Dob"
            });

            query.MemberCollection.AddRange("Dob", "FullName", "Gender", "Race");

            var json = await query.GenerateEsQueryAsync();
            var jo = JObject.Parse(json);
            var fields = jo.SelectToken("$.fields").Values<string>().ToArray();
            Assert.Contains("Dob", fields);
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData("Roles", "Administrators")]
        [InlineData("Designation", "Senior Manager")]
        public async Task QueryFields(string performer, string performerValues)
        {
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = "Patient",
                WebId = "all-born-in-60s"
            };
            var fields = new[] { "Dob", "FullName", "Gender", "Race" };
            query.MemberCollection.AddRange(fields);

            var json = await query.GenerateEsQueryAsync();
            var jo = JObject.Parse(json);
            var esFields = jo.SelectToken("$.fields").Values<string>().ToArrayString();

            Assert.Equal(fields, esFields);
        }
    }
}
