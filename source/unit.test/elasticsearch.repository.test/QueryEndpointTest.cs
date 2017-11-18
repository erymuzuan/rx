using System;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    [Trait("Category", "Query endpoints")]
    [Collection("Endpoint")]
    public class QueryEndpointTest
    {
        private ITestOutputHelper Console { get; }

        public QueryEndpointTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        [Trait("Query", "Elasticsearch")]
        public void CompileQueryFieldsAndFilter()
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

            var dsl = query.QueryDsl;

            var json = new Patient().CompileToElasticsearchQueryDsl(dsl);
            var jo = JObject.Parse(json);
            var fields = jo.SelectToken("$._source").Values<string>().ToArray();
            Assert.Contains("Dob", fields);
            Console.WriteLine(fields.ToString("\r\n"));
        }
        [Theory]
        [InlineData(null, null)]
        [InlineData("Roles", "Administrators")]
        [InlineData("Designation", "Senior Manager")]
        public void QueryFields(string performer, string performerValues)
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

            var json = default(Patient).CompileToElasticsearchQueryDsl(query.QueryDsl);
            var jo = JObject.Parse(json);
            var esFields = jo.SelectToken("$._source").Values<string>().ToArrayString();

            Assert.Equal(fields, esFields);
        }
    }
}
